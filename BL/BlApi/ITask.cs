

namespace BlApi;
/// <summary>
///     
/// </summary>
public interface ITask
{
    public int Create(BO.Task boTask);
    public BO.Task? Read(int TaskId);
    public IEnumerable<BO.TaskInList> ReadAll(Func<DO.Task, bool>? filter = null);
    public void Update(BO.Task boTask);
    //uses update designate an engineer to the task
    public void DesignateEngineer(int taskId, int engineerId);

    //define the project schedule and produce milestones
    //engineer update only progress(status)
    public void Delete(int TaskId);
    //get list of tasks of a given engineer

    public void Update(int TaskId, DateTime _ScheduledDate);


}
