﻿namespace BlImplementation;
using BlApi;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    private readonly Bl _bl;
    internal TaskImplementation(Bl bl) => _bl = bl;


    //Method to calculate the status of a given DO.Task 
    private BO.Status GetStatus(int id)
    {
        DO.Task task = _dal.Task.Read(id) ?? throw new BO.BlDoesNotExistException("The task doesn't exist");
        if (task.ScheduledDate == null)
            return BO.Status.Unscheduled;
        else if (task.StartDate == null)
            return BO.Status.Scheduled;
        else if (task.CompleteDate != null)
            return BO.Status.Done;
        else if (task.ScheduledDate + task.RequiredEffortTime > _bl.Clock)
            return BO.Status.OnTrack;
        else
            return BO.Status.InJeopardy;
    }

    //returns a list of dependencies
    private List<BO.TaskInList> GetDependencies(int taskId)
    {
        DO.Task doTask = _dal.Task.Read(taskId) ?? throw new BO.BlDoesNotExistException($"Task with ID: {taskId} does not exist");

        IEnumerable<int?> idTasks = _dal.Dependency.ReadAll().Where(d => d != null).Where(d => d!.DependentTask == doTask.Id).Select(d => d!.DependsOnTask);
        return (from int idTask in idTasks
                select new BO.TaskInList
                {
                    Id = idTask,
                    Description = _dal.Task.Read(idTask)!.Description,
                    Alias = _dal.Task.Read(idTask)!.Alias,
                    Status = GetStatus(idTask)
                }).ToList();
    }

    //returns the engineer in charge of the task
    private BO.EngineerInTask? GetEngineerInTask(int taskId)
    {
        DO.Task doTask = _dal.Task.Read(taskId) ?? throw new BO.BlDoesNotExistException($"Task with ID: {taskId} does not exist");

        if (doTask.EngineerId == null)
            return null;
        DO.Engineer doEngineer = _dal.Engineer.Read((int)doTask.EngineerId) ?? throw new BO.BlDoesNotExistException($"Engineer with ID: {(int)doTask.EngineerId} does not exist");
        return new BO.EngineerInTask()
        {
            Id = doEngineer.Id,
            Name = doEngineer.Name
        };
    }

    //returns the id of the first task that the schedule wasn't initialized
    private int LeastDependentTask(int taskId)
    {
        List<BO.TaskInList>? dependencies = Read(taskId).Dependencies;
        bool scheduled = dependencies.All(d => Read(d.Id).Status != BO.Status.Unscheduled);
        if (dependencies == null || scheduled)
            return taskId;
        IEnumerable<int> leastDependentTasks = from dependency in dependencies
                                               let leastDependentTask = LeastDependentTask(dependency.Id)
                                               select leastDependentTask;
        return leastDependentTasks.FirstOrDefault();


    }

    //returns the latest forcast date of the depencies (from the given list)
    private DateTime? MaxForcastDate(List<BO.TaskInList> tasks)
    {
        if (tasks.Count == 0) return _bl.Clock;
        return tasks.Max(t => Read(t.Id).ForecastDate);
    }

    //saves the task to the database
    public int Create(BO.Task boTask)
    {
        if (_dal.Task.GetStatus() >= 2)
            throw new BO.BlScheduled("A new task cannot be added after the schedule initialization has started");
        if (boTask.Id <= 0)
            throw new BO.InCorrectData("Task ID must be a positive integer");
        if (boTask.Alias == "")
            throw new BO.InCorrectData("Task should have an alias");
        DO.Task doTask = new
        (
            boTask.Id,
            boTask.Alias,
            boTask.Description,
            boTask.CreatedAtDate,
            (DO.EngineerExperience)boTask.Complexity,
            boTask.Deliverables,
            boTask.Remarks,
            false,
            boTask.RequiredEffortTime,
            null,
            null,
            null,
            null,
            null
        );
        int TaskId = _dal.Task.Create(doTask);
        boTask.Dependencies.ForEach(dependency =>
            {
                DO.Dependency doDependency = new(0, TaskId, dependency.Id);
                _dal.Dependency.Create(doDependency);
            });

        return TaskId;
    }

    //removes the task from the database
    public void Delete(int taskId)
    {
        if (_dal.Task.GetStartDate() != null)
            throw new BO.BlDeletionImpossible("A task cannot be deleted at this stage");
        if (_dal.Task.Read(taskId) == null)
            throw new BO.BlDoesNotExistException($"Task with ID: {taskId} does not exist");
        if (_dal.Dependency.ReadAll(d => d.DependsOnTask == taskId).Any())
            throw new BO.BlDeletionImpossible("Cannot be deleted due to other tasks depending on it");
        _dal.Task.Delete(taskId);
    }

    //assigns the given engineer to the given task
    public void DesignateEngineer(int taskId, int engineerId)
    {
        if (_dal.Task.GetStatus() < 3 || _dal.Task.GetStatus() == null)
            throw new BO.BlUnScheduled("An engineer cannot be assigned before schedule initialization is complete for all tasks");
        BO.Task boTask = Read(taskId);
        DO.Engineer engineer = _dal.Engineer.Read(engineerId) ?? throw new BO.BlDoesNotExistException($"Engineer with ID: {engineerId} does not exist");
        bool flag = (_dal.Task.ReadAll().Where(task => task != null).All(task => !(task!.EngineerId == engineerId && task!.CompleteDate == null)));
        if (!flag)
            throw new BO.BlEngineerIsAlreadyOccupied($"Engineer with ID: {engineerId} is currently assigned with a task.");
        if ((int)boTask.Complexity > (int)engineer.Level) throw new BO.BlEngineerLevelIsTooLow($"Engineer with ID: {engineerId} doesn't have the expertise required for the task.");


        DO.Task doTask = new
     (
         boTask.Id,
         boTask.Alias,
         boTask.Description,
         boTask.CreatedAtDate,
         (DO.EngineerExperience)boTask.Complexity,
         boTask.Deliverables,
         boTask.Remarks,
         false,
         boTask.RequiredEffortTime,
         boTask.StartDate,
         boTask.ScheduledDate,
         boTask.DeadlineDate,
         boTask.CompleteDate,
         engineerId
     );
        try
        {
            _dal.Task.Update(doTask);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID: {taskId} does not exist", ex);
        }
    }

    //returns the task
    public BO.Task Read(int taskId)
    {
        DO.Task? doTask = _dal.Task.Read(taskId) ?? throw new BO.BlDoesNotExistException($"Task with ID: {taskId} does not exist");
        DateTime? forecastDate = null;
        if (doTask.ScheduledDate != null)
            forecastDate = doTask.ScheduledDate + doTask.RequiredEffortTime;

        return (new BO.Task
        {
            Id = taskId,
            Alias = doTask.Alias,
            Description = doTask.Description,
            CreatedAtDate = doTask.CreatedAtDate,
            Status = GetStatus(doTask.Id),
            Dependencies = GetDependencies(doTask.Id),
            Complexity = (BO.EngineerExperience)doTask.Complexity,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            RequiredEffortTime = doTask.RequiredEffortTime,
            StartDate = doTask.StartDate,
            ScheduledDate = doTask.ScheduledDate,
            ForecastDate = forecastDate,
            DeadlineDate = doTask.DeadlineDate,
            CompleteDate = doTask.CompleteDate,
            Engineer = GetEngineerInTask(doTask.Id)
        });

    }

    //returns all task in a short fasion
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        IEnumerable<BO.TaskInList> boTasksInList = (from DO.Task doTask in _dal.Task.ReadAll()
                                                    select new BO.TaskInList
                                                    {
                                                        Id = doTask.Id,
                                                        Description = doTask.Description,
                                                        Alias = doTask.Alias,
                                                        Status = GetStatus(doTask.Id)
                                                    });
        if (filter != null)
            return from boTask in boTasksInList
                   where filter(_bl.Task.Read(boTask.Id)!)
                   select boTask;
        else
            return boTasksInList;

    }

    //updates a given task
    public void Update(BO.Task boTask)
    {
        if (_dal.Task.GetStartDate() == null)
            throw new BO.BlScheduled("These details cannot be updated at this time for tasks");
        // if (_dal.Task.GetStatus() <= 2)
        //    throw new BO.BlScheduled("These details cannot be updated at this time for tasks");

        if (boTask.Id < 0)
            throw new BO.InCorrectData("Task ID can't be negative");
        if (boTask.Alias == "")
            throw new BO.InCorrectData("Task should have an alias");
        DO.Task doTask = new(
            boTask.Id,
            boTask.Alias,
            boTask.Description,
            boTask.CreatedAtDate,
            (DO.EngineerExperience)boTask.Complexity,
            boTask.Deliverables,
            boTask.Remarks,
            false,
            boTask.RequiredEffortTime,
            boTask.StartDate,
            boTask.ScheduledDate,
            boTask.DeadlineDate,
            boTask.CompleteDate,
            boTask.Engineer != null ? boTask.Engineer!.Id : null
        );

        try
        {
            _dal.Task.Update(doTask);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID: {doTask.Id} does not exist", ex);
        }
        IEnumerable<int> tempDependencies = from taskInList in boTask.Dependencies
                                            let dependency = new DO.Dependency(0, boTask.Id, taskInList.Id)
                                            select _dal.Dependency.Create(dependency);
    }

    //updates the schedule date
    public void Update(int taskId, DateTime _scheduledDate)
    {
        if (_dal.Task.GetStartDate() == null)
            throw new BO.BlUnScheduled("The task schedule date cannot be initialized before the project start date is received");
        if (_dal.Task.GetStartDate() > _scheduledDate)
            throw new BO.BlUpdateImpossible($"Start date id {_dal.Task.GetStartDate()}.\nCan't  set schedule to be Before project start date ");

        BO.Task? boTask = null;
        boTask = Read(taskId);
        if (!(boTask.Dependencies.All(d => d.Status != BO.Status.Unscheduled)))
            throw new BO.BlUpdateImpossible($"The previous tasks weren't scheduled, you must schduele the task with ID: {LeastDependentTask(boTask.Id)}");
        if (!(boTask.Dependencies.All(d => Read(d.Id).ForecastDate <= _scheduledDate)))
            throw new BO.BlUpdateImpossible($"The previous tasks must be complete before the current task, the date must be set after {MaxForcastDate(boTask.Dependencies)}");
        boTask.ScheduledDate = _scheduledDate;

        DO.Task doTask = new
     (
         boTask.Id,
         boTask.Alias,
         boTask.Description,
         boTask.CreatedAtDate,
         (DO.EngineerExperience)boTask.Complexity,
         boTask.Deliverables,
         boTask.Remarks,
         false,
         boTask.RequiredEffortTime,
         null,
         _scheduledDate,
         null,
         null,
         null
     );

        try
        {
            _dal.Task.Update(doTask);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID: {doTask.Id} does not exist", ex);
        }
    }

    //only updates what the user is allowed to in stage 3
    public void AddDependency(int taskId, int boTask)//setter
    {
        if (taskId == boTask)
            throw new BO.BlUpdateImpossible("Cannot depend on himself");
        if (_dal.Task.Read(taskId) == null)
            throw new BO.BlDoesNotExistException($"Task with ID: {taskId} does not exist");
        if (_dal.Task.Read(boTask) == null)
            throw new BO.BlDoesNotExistException($"Task with ID: {boTask} does not exist");
        if (_dal.Task.GetStatus() >= 2)
            throw new BO.BlScheduled("Can't add dependencies once you completed setting the tasks.");
        DO.Dependency doDependency = new(0, taskId, boTask);
        foreach (DO.Dependency dependency in _dal.Dependency.ReadAll())
        {
            if (dependency != null && dependency.DependentTask == doDependency.DependentTask &&
                dependency.DependsOnTask == doDependency.DependsOnTask)
                throw new BO.BlAlreadyExistsException("Dependency already exists");
        }
        try
        {
            _dal.Dependency.Create(doDependency);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID: {doDependency.Id} does not exist", ex);
        }
    }

    //sets the project to be in the first stage
    public void SetStage1()
    {
        int? projectStatus = _dal.Task.GetStatus();
        if (!projectStatus.HasValue) { _dal.Task.IncreaseStatus(); }



        if (projectStatus > 1) throw new BO.BlScheduled($"the project is already up to stage {projectStatus}.");


    }

    //sets the project to be in the second stage

    public void SetStage2(DateTime startDate)
    {
        int? projectStatus = _dal.Task.GetStatus();
        if (projectStatus >= 2) throw new BO.BlScheduled($"the project is already up to stage {projectStatus}.");
        if (!projectStatus.HasValue) throw new BO.BlUnScheduled($"the project is not ready for stage 2.");
        if (projectStatus == 1)
        {
            _dal.Task.IncreaseStatus();
            _dal.Task.SetStartDate(startDate);
        }
    }
    //sets the project to be in the third stage

    public void SetStage3()
    {
        int? projectStatus = _dal.Task.GetStatus();
        if (!projectStatus.HasValue) throw new BO.BlUnScheduled($"the project is not ready for stage 3.");
        if (projectStatus == 1) throw new BO.BlUnScheduled($"the project is not ready for stage 3.");
        if (projectStatus == 2)
        {
            bool flag = (ReadAll().All(task => task.Status != BO.Status.Unscheduled));
            if (!flag) throw new BO.BlUnScheduled($"the project is not ready for stage 3. initiate all scheduled dates.");
            _dal.Task.IncreaseStatus();
        }
    }
    //setter for complete date
    public void SetCompleteDate(int taskId, DateTime? _completeDate = null)
    {
        if (_dal.Task.GetStatus() < 3)
            throw new BO.BlUpdateImpossible("Tasks cannot be completed at this stage");
        BO.Task boTask = Read(taskId);
        if (boTask.StartDate == null)
            throw new BO.BlUpdateImpossible("A task cannot be completed before it starts");
        if (boTask.CompleteDate != null)
            throw new BO.BlUpdateImpossible("the task is complete already");
        if (_completeDate == null)
        {
            _completeDate = _bl.Clock;
        }
        DO.Task doTask = new(
         boTask.Id,
         boTask.Alias,
         boTask.Description,
         boTask.CreatedAtDate,
         (DO.EngineerExperience)boTask.Complexity,
         boTask.Deliverables,
         boTask.Remarks,
         false,
         boTask.RequiredEffortTime,
         boTask.StartDate,
         boTask.ScheduledDate,
         boTask.DeadlineDate,
         _completeDate,
         boTask.Engineer!.Id
     );

        try
        {
            _dal.Task.Update(doTask);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID: {doTask.Id} does not exist", ex);
        }
    }

    //setter for start date

    public void SetStartDate(int taskId, DateTime? _startDate = null)
    {
        if (_dal.Task.GetStatus() < 3)
            throw new BO.BlUnScheduled("Tasks cannot be started at this stage");
        BO.Task boTask = Read(taskId);
        if (boTask.Engineer == null)
            throw new BO.BlUpdateImpossible("A task cannot be started before an engineer has been assigned to it");
        if (boTask.ScheduledDate == null)
            throw new BO.BlUnScheduled("Tasks cannot be started at this stage");
        if (boTask.CompleteDate != null)
            throw new BO.BlUpdateImpossible("the task is complete");
        if (_startDate == null || _startDate < boTask.ScheduledDate)
        {
            _startDate = _bl.Clock;
        }
        DO.Task doTask = new
        (
            boTask.Id,
            boTask.Alias,
            boTask.Description,
            boTask.CreatedAtDate,
            (DO.EngineerExperience)boTask.Complexity,
            boTask.Deliverables,
            boTask.Remarks,
            false,
            boTask.RequiredEffortTime,
            _startDate,
            boTask.ScheduledDate,
            boTask.DeadlineDate,
            boTask.CompleteDate,
            boTask.Engineer.Id
        );
        try
        {
            _dal.Task.Update(doTask);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID: {doTask.Id} does not exist", ex);
        }
    }

    //returns stage of the project
    public int? GetProjectStatus()
    {
        return _dal.Task.GetStatus();
    }

    //increases project status
    public void IncreaseStatus()
    {
        _dal.Task.IncreaseStatus();
    }

    public DateTime? GetFirstScheduledDate()
    {
        var minDate = (from doTask in _dal.Task.ReadAll()
                       select doTask.ScheduledDate).Min();

        return minDate;
    }

    public DateTime? GetProjectStartDate()
    {
        return _dal.Task.GetStartDate();
    }

    public void AutoSchedule(DateTime firstDate)
    {
        if (GetProjectStatus() != 2)
            throw new BO.BlUpdateImpossible("You cannot edit the schedule at this time!");
        if (GetProjectStartDate() == null)
            throw new BO.BlUpdateImpossible("You must initialize the project start date!");
        else if (GetProjectStartDate() > firstDate)
            throw new BO.BlUpdateImpossible("You must choose a date after the project start date!");
        else
        {
            foreach (var task in ReadAll())
            {
                Update(task.Id, (DateTime)MaxForcastDate(Read(task.Id)!.Dependencies)!);
            }
            SetStage3();
        }
    }
}
