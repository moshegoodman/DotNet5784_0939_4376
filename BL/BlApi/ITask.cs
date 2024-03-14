

namespace BlApi;
/// <summary>
///     
/// </summary>
public interface ITask
{
    public int Create(BO.Task boTask);//saves a task to the database
    public BO.Task? Read(int TaskId);//returns a task with a given id
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool>? filter = null);//returns all tasks(in a short format)
    public void Update(BO.Task boTask);
    //uses update designate an engineer to the task
    public void UpdateStage3(BO.Task boTask);//updates only what is allowed to on the third stage
    public void DesignateEngineer(int idTask, int engineerId);//designates an engineer to the task

    public void Delete(int TaskId);//removes a given task from the database

    public void Update(int TaskId, DateTime _ScheduledDate);//updates scheduled date
    public void SetStartDate(int taskId, DateTime? _startDate = null);//setter

    public void SetCompleteDate(int taskId, DateTime? _completeDate = null);//setter
    public void AddDependency(int taskId, int boTask);//setter







    //------------------------------------------
    public void IncreaseStatus();//increases the PROJECT status

    public int? GetProjectStatus();//returns project status
    public void SetStage1();//sets the project status to 1
    public void SetStage2(DateTime startDate);//sets the project status to 2
    public void SetStage3();//sets the project status to 3

    public DateTime? GetFirstScheduledDate();

    public DateTime? GetProjectStartDate();

}
