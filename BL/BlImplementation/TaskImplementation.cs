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

    private List<BO.TaskInList> GetDependencies(int id)
    {
        DO.Task? task = _dal.Task.Read(id);

        IEnumerable<int?> idTasks = _dal.Dependency.ReadAll().Where(d => d != null).Where(d => d!.DependentTask == task.Id).Select(d => d!.DependsOnTask);
        return (from int idTask in idTasks
                select new BO.TaskInList
                {
                    Id = idTask,
                    Description = _dal.Task.Read(idTask)!.Description,
                    Alias = _dal.Task.Read(idTask)!.Alias,
                    Status = GetStatus(_dal.Task.Read(idTask)!)
                }).ToList();
    }

    private BO.EngineerInTask? GetEngineerInTask(int id)
    {
        DO.Task? task = _dal.Task.Read(id);

        if (task.EngineerId == null)
            return null;
        return new BO.EngineerInTask()
        {
            Id = (int)task.EngineerId,
            Name = _dal.Engineer.Read((int)task.EngineerId)!.Name
        };
    }

    public int Create(BO.Task boTask)
    {
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

    public void Delete(int id)
    {
        if (_dal.Task.Read(id) == null)
            throw new BO.BlDoesNotExistException($"Task with ID: {id} does not exist");
        if (_dal.Dependency.ReadAll(d => d.DependsOnTask == id).Any())
            throw new BO.BlDeletionImpossible("Cannot be deleted due to other tasks depending on it");
        _dal.Task.Delete(id);
    }

    public void DesignateEngineer(int taskId, int engineerId)
    {
        BO.Task? boTask = Read(taskId);
        DO.Engineer engineer = _dal.Engineer.Read(engineerId);
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
        _dal.Task.Update(doTask);
    }

    public BO.Task Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask == null)
            throw new BO.BlDoesNotExistException($"Task with ID: {id} does not exist");
        return (new BO.Task
        {
            Id = id,
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

    public IEnumerable<BO.TaskInList> ReadAll(Func<DO.Task, bool>? filter = null)//Make shure that we can use this type for the filter
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
            return (from DO.Task doTask in _dal.Task.ReadAll()
                    where filter(doTask)
                    select new BO.TaskInList
                    {
                        Id = doTask.Id,
                        Description = doTask.Description,
                        Alias = doTask.Alias,
                        Status = GetStatus(doTask.Id)
                    });

        }
    }
    public void Update(BO.Task boTask)
    {
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

    public void Update(int id, DateTime _scheduledDate)
    {
        BO.Task? boTask = null;
        try
        {
            boTask = Read(id);
        }
        catch (BO.BlDoesNotExistException ex)
        {
            throw ex;
        }
        if (!(boTask.Dependencies.All(d => boTask.ScheduledDate != null)))
            throw new BO.BlUpdateImpossible("The previous tasks weren't scheduled");
        if (!(boTask.Dependencies.All(d => boTask.ForecastDate < _scheduledDate)))
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
}
