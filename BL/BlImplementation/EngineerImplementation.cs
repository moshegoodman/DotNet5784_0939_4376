namespace BlImplementation;
using BlApi;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    //getter 
    private BO.TaskInEngineer? GetTaskInEngineer(int EngineerId)
    {
        DO.Task? doTask = _dal.Task.ReadAll().Where(task => task != null).Where(task => task!.EngineerId == EngineerId).Where(task => task!.CompleteDate == null).FirstOrDefault();
        if (doTask == null)
            return null;
        BO.TaskInEngineer? taskInEngineer = new BO.TaskInEngineer()
        {
            Id = doTask.Id,
            alias = doTask.Alias
        };
        return taskInEngineer;
    }

    //saves the object in the database
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
                (boEngineer.Id, boEngineer.Email, boEngineer.Cost, boEngineer.Name, (DO.EngineerExperience)boEngineer.Level);

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

    //removes object from the database
    public void Delete(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id) ?? throw new BO.BlDoesNotExistException($"Engineer with ID={id} does Not exist");
        if (Read(id)!.Task != null)
            throw new BO.BlDeletionImpossible("The engineer is in the middle of performing the task or has already finished the task");
        _dal.Engineer.Delete(id);

    }

    //user gives id the method returns engineer
    public BO.Engineer? Read(int id)
    {

        DO.Engineer? doEngineer = _dal.Engineer.Read(id) ?? throw new BO.BlDoesNotExistException($"Engineer with ID={id} does Not exist");
        return (new BO.Engineer
        {
            Id = id,
            Name = doEngineer.Name,
            Cost = doEngineer.Cost,
            Email = doEngineer.Email,
            Level = (BO.EngineerExperience)doEngineer.Level,
            Task = GetTaskInEngineer(id)
        });
    }

    //returns all the engineers
    public IEnumerable<BO.Engineer>? ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        IEnumerable<BO.Engineer> boEngineers = (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                                                select new BO.Engineer
                                                {
                                                    Id = doEngineer.Id,
                                                    Name = doEngineer.Name,
                                                    Cost = doEngineer.Cost,
                                                    Email = doEngineer.Email,
                                                    Level = (BO.EngineerExperience)doEngineer.Level,
                                                    Task = GetTaskInEngineer(doEngineer.Id)
                                                });
        if (filter != null)
            return from boEngineer in boEngineers
                   where filter(boEngineer)
                   select boEngineer;
        else
            return boEngineers;

    }

    //updates all fields in the 
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
        if ((int)boEngineer.Level < (int)_dal.Engineer.Read(boEngineer.Id)!.Level)
            throw new BO.BlUpdateImpossible("Engineer level cannot decrease");
        DO.Engineer doEngineer = new DO.Engineer
                (boEngineer.Id, boEngineer.Email, boEngineer.Cost, boEngineer.Name, (DO.EngineerExperience)boEngineer.Level);
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

    //setter for level
    public void UpdateLevel(int engineerId, BO.EngineerExperience level)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(engineerId);
        if (doEngineer == null) throw new BO.BlDoesNotExistException($"Engineer ID {engineerId} does not exist");
        doEngineer = new(doEngineer.Id, doEngineer.Email, doEngineer.Cost, doEngineer.Name, (DO.EngineerExperience)level);
        _dal.Engineer.Update(doEngineer);
    }

    //setter for email
    public void SetEmail(int engineerId, string email)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(engineerId);
        if (doEngineer == null) throw new BO.BlDoesNotExistException($"Engineer ID {engineerId} does not exist");
        doEngineer = new(doEngineer.Id, email, doEngineer.Cost, doEngineer.Name, doEngineer.Level);
        _dal.Engineer.Update(doEngineer);
    }

    //setter for name
    public void SetName(int engineerId, string name)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(engineerId);
        if (doEngineer == null) throw new BO.BlDoesNotExistException($"Engineer ID {engineerId} does not exist");
        doEngineer = new(doEngineer.Id, doEngineer.Email, doEngineer.Cost, name, doEngineer.Level);
        _dal.Engineer.Update(doEngineer);
    }

    //setter for cost
    public void SetCost(int engineerId, double cost)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(engineerId);
        if (doEngineer == null) throw new BO.BlDoesNotExistException($"Engineer ID {engineerId} does not exist");
        doEngineer = new(doEngineer.Id, doEngineer.Email, cost, doEngineer.Name, doEngineer.Level);
        _dal.Engineer.Update(doEngineer);
    }
}


