namespace BlImplementation;
using BlApi;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    private BO.TaskInEngineer GetTaskInEngineer(int EngineerId) => new BO.TaskInEngineer()
    {
        Id = _dal.Task.ReadAll().Where(task => task != null).Where(task => task!.EngineerId == EngineerId).Where(task => task!.CompleteDate == null).FirstOrDefault()!.Id,
        alias = _dal.Task.ReadAll().Where(task => task != null).Where(task => task!.EngineerId == EngineerId).Where(task => task!.CompleteDate == null).FirstOrDefault()!.Alias
    };
    public int Create(BO.Engineer boEngineer)
    {
        if (boEngineer.Id < 0)
            throw new BO.InCorrectData("Engineer ID cant be negative");
        if (!(boEngineer.Email.Contains('@') && boEngineer.Email.Contains('.') && boEngineer.Email.IndexOf("@") < boEngineer.Email.IndexOf(".")))
            throw new BO.InCorrectData($"Email: {boEngineer.Email} is invalid");
        if (boEngineer.Cost < 0)
            throw new BO.InCorrectData("Engineer cost cant be negative");
        if (boEngineer.Name == "")
            throw new BO.InCorrectData("Engineer should have a name");
        DO.Engineer doEngineer = new DO.Engineer
                (boEngineer.Id, boEngineer.Email, boEngineer.Cost, boEngineer.Name, boEngineer.Level);

        try
        {
            int idEngineer = _dal.Engineer.Create(doEngineer);
            return idEngineer;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={boEngineer.Id} already exists", ex);
        }
    }

    public void Delete(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id) ?? throw new BO.BlDoesNotExistException($"Engineer with ID={id} does Not exist");
        if (Read(id)!.Task != null)
            throw new BO.BlDeletionImpossible("The engineer is in the middle of performing the task or has already finished the task");
        _dal.Engineer.Delete(id);

    }

    public BO.Engineer? Read(int id)
    {

        DO.Engineer? doEngineer = _dal.Engineer.Read(id) ?? throw new BO.BlDoesNotExistException($"Engineer with ID={id} does Not exist");
        return new BO.Engineer()
        {
            Id = id,
            Name = doEngineer.Name,
            Cost = doEngineer.Cost,
            Email = doEngineer.Email,
            Level = doEngineer.Level,
            Task = GetTaskInEngineer(id)
        };
    }

    public IEnumerable<BO.Engineer>? ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        IEnumerable<BO.Engineer> boEngineers = (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                                                select new BO.Engineer
                                                {
                                                    Id = doEngineer.Id,
                                                    Name = doEngineer.Name,
                                                    Cost = doEngineer.Cost,
                                                    Email = doEngineer.Email,
                                                    Level = doEngineer.Level,
                                                    Task = GetTaskInEngineer(doEngineer.Id)
                                                });
        if (filter != null)
            return from boEngineer in boEngineers
                   where filter(boEngineer)
                   select boEngineer;
        else
            return boEngineers;

    }

    public void Update(BO.Engineer boEngineer)
    {
        if (boEngineer.Id < 0)
            throw new BO.InCorrectData("Engineer ID cant be negative");
        if (!(boEngineer.Email.Contains("@") && boEngineer.Email.Contains(".") && boEngineer.Email.IndexOf("@") < boEngineer.Email.IndexOf(".")))
            throw new BO.InCorrectData($"Email: {boEngineer.Email} is invalid");
        if (boEngineer.Cost < 0)
            throw new BO.InCorrectData("Engineer cost cant be negative");
        if (boEngineer.Name == "")
            throw new BO.InCorrectData("Engineer should have a name");
        if (boEngineer.Level < _dal.Engineer.Read(boEngineer.Id)!.Level)
            throw new BO.BlUpdateImpossible("Engineer level cannot decrease");
        DO.Engineer doEngineer = new DO.Engineer
                (boEngineer.Id, boEngineer.Email, boEngineer.Cost, boEngineer.Name, boEngineer.Level);
        BO.TaskInEngineer? taskInEngineer = boEngineer.Task;
        if (taskInEngineer != null)
        {
            if (_dal.Task.Read(taskInEngineer.Id) == null)
                throw new BO.BlUpdateImpossible($"Task with ID={taskInEngineer.Id} does Not exist");
            if (_dal.Task.Read(taskInEngineer.Id)!.EngineerId != null)
                throw new BO.BlUpdateImpossible($"Task was already allocated to an engineer");
            DO.Task newTask = _dal.Task.Read(taskInEngineer.Id)! with { EngineerId = boEngineer.Id };
            _dal.Task.Update(newTask);
        }

        try
        {
            _dal.Engineer.Update(doEngineer);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Engineer with ID={boEngineer.Id} does Not exist", ex);
        }
    }
}
