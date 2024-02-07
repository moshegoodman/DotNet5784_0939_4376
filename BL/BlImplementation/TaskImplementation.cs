namespace BlImplementation;
using BlApi;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    //Method to calculate the status of a given DO.Task 
    private BO.Status GetStatus(int id)
    {
        DO.Task? task = _dal.Task.Read(id);

        if (task == null)
            throw new BO.BlDoesNotExistException("The task doesn't exist");
        if (task.ScheduledDate == null)
            return BO.Status.Unscheduled;
        else if (task.StartDate == null)
            return BO.Status.Scheduled;
        else if (task.CompleteDate != null)
            return BO.Status.Done;
        else if (task.DeadlineDate > DateTime.Now)
            return BO.Status.OnTrack;
        else
            return BO.Status.InJeopardy;
    }

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

    public int Create(BO.Task boTask)
    {
        if (_dal.Task.GetStartDate() != null)
            throw new BO.BlScheduled("A new task cannot be added after the schedule initialization has started");
        if (boTask.Id < 0)
            throw new BO.InCorrectData("Task ID can't be negative");
        if (boTask.Alias == "")
            throw new BO.InCorrectData("Task should have an alias");
        DO.Task doTask = new DO.Task
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

        IEnumerable<int> tempDependencies = from taskInList in boTask.Dependencies
                                            let dependency = new DO.Dependency(0, boTask.Id, taskInList.Id)
                                            select _dal.Dependency.Create(dependency);

        return _dal.Task.Create(doTask);
    }

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

    public void DesignateEngineer(int taskId, int engineerId)
    {
        if (_dal.Task.GetStatus() < 3)
            throw new BO.BlUnScheduled("An engineer cannot be assigned before schedule initialization is complete for all tasks");
        BO.Task boTask = Read(taskId);
        DO.Engineer engineer = _dal.Engineer.Read(engineerId) ?? throw new BO.BlDoesNotExistException($"Engineer with ID: {engineerId} does not exist");
        bool flag = (_dal.Task.ReadAll().Where(task => task != null).All(task => !(task!.EngineerId == engineerId && task!.CompleteDate == null)));
        if (!flag)
            throw new BO.BlEngineerIsAlreadyOccupied($"Engineer with ID: {engineerId} is currently assigned with a task.");
        DO.Task doTask = new DO.Task
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

    public BO.Task Read(int taskId)
    {
        DO.Task? doTask = _dal.Task.Read(taskId) ?? throw new BO.BlDoesNotExistException($"Task with ID: {taskId} does not exist");
        return (new BO.Task
        {
            Id = taskId,
            Alias = doTask.Alias,
            Description = doTask.Description,
            CreatedAtDate = doTask.CreatedAtDate,
            Status = GetStatus(doTask.Id),
            Dependencies = GetDependencies(doTask.Id),
            Milestone = null,
            Complexity = (BO.EngineerExperience)doTask.Complexity,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            RequiredEffortTime = doTask.RequiredEffortTime,
            StartDate = doTask.StartDate,
            ScheduledDate = doTask.ScheduledDate,
            ForecastDate = doTask.StartDate + doTask.RequiredEffortTime,
            DeadlineDate = doTask.DeadlineDate,
            CompleteDate = doTask.CompleteDate,
            Engineer = GetEngineerInTask(doTask.Id)
        });

    }

    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool>? filter = null)//Make shure that we can use this type for the filter
    {
        if (filter == null)
        {
            return (from DO.Task doTask in _dal.Task.ReadAll()
                    select new BO.TaskInList
                    {
                        Id = doTask.Id,
                        Description = doTask.Description,
                        Alias = doTask.Alias,
                        Status = GetStatus(doTask.Id)
                    });
        }
        else
        {
            return (from BO.Task boTask in _dal.Task.ReadAll()
                    where filter(boTask)
                    select new BO.TaskInList
                    {
                        Id = boTask.Id,
                        Description = boTask.Description,
                        Alias = boTask.Alias,
                        Status = GetStatus(boTask.Id)
                    });

        }
    }
    public void Update(BO.Task boTask)
    {
        if (_dal.Task.GetStartDate() != null)
            throw new BO.BlScheduled("These details cannot be updated at this time for tasks");
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
            null,
            null,
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
        IEnumerable<int> tempDependencies = from taskInList in boTask.Dependencies
                                            let dependency = new DO.Dependency(0, boTask.Id, taskInList.Id)
                                            select _dal.Dependency.Create(dependency);
    }

    public void Update(int taskId, DateTime _scheduledDate)
    {
        if (_dal.Task.GetStartDate() == null)
            throw new BO.BlUnScheduled("The task schedule date cannot be initialized before the project start date is received");
        BO.Task? boTask = null;
        boTask = Read(taskId);
        if (!(boTask.Dependencies.All(d => d.Status != BO.Status.Unscheduled)))
            throw new BO.BlUpdateImpossible("The previous tasks weren't scheduled");
        if (!(boTask.Dependencies.All(d => Read(d.Id).ForecastDate <= _scheduledDate)))
            throw new BO.BlUpdateImpossible("The previous tasks must be complete before the current task");
        boTask.ScheduledDate = _scheduledDate;

        DO.Task doTask = new DO.Task
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

















































    public void UpdateStage3(BO.Task boTask)
    {
        if (_dal.Task.GetStatus() < 3)
            throw new BO.BlScheduled("These details cannot be updated at this stage for tasks");
        if (boTask.Id < 0)
            throw new BO.InCorrectData("Task ID can't be negative");
        if (boTask.Alias == "")
            throw new BO.InCorrectData("Task should have an alias");
        DO.Task doTask = _dal.Task.Read(boTask.Id) ?? throw new BO.BlDoesNotExistException($"Task with ID: {boTask.Id} does not exist");


        doTask = new(

            doTask.Id,
            boTask.Alias,
            boTask.Description,
            boTask.CreatedAtDate,
            doTask.Complexity,
            boTask.Deliverables,
            boTask.Remarks,
            false,
            doTask.RequiredEffortTime,
            doTask.StartDate,
            doTask.ScheduledDate,
            doTask.DeadlineDate,
            doTask.CompleteDate,
            doTask.EngineerId
        );


        _dal.Task.Update(doTask);

    }


    public void SetStartDate(int taskId, DateTime? _startDate = null)
    {
        if (_dal.Task.GetStatus() < 3)
            throw new BO.BlUnScheduled("Tasks cannot be started at this stage");
        BO.Task boTask = Read(taskId);
        if (boTask.Engineer == null)
            throw new BO.BlUpdateImpossible("A task cannot be started before an engineer has been assigned to it");
        if(boTask.ScheduledDate == null)
            throw new BO.BlUnScheduled("Tasks cannot be started at this stage");
        if (boTask.CompleteDate != null)
            throw new BO.BlUpdateImpossible("the task is complete");
        if (_startDate == null || _startDate < boTask.ScheduledDate)
        {
            _startDate = DateTime.Now;
        }
        DO.Task doTask = new DO.Task
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
            _completeDate = DateTime.Now;
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
         boTask.CompleteDate,
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
}
