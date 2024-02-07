

namespace BlApi;
/// <summary>
///     
/// </summary>
public interface ITask
{
    public int Create(BO.Task item);
    public BO.Task? Read(int id);
    public IEnumerable<BO.TaskInList> ReadAll(Func<DO.Task, bool>? filter = null);
    public void Update(BO.Task item);
    //uses update designate an engineer to the task
    public void DesignateEngineer(int , int);

    //define the project schedule and produce milestones
    //engineer update only progress(status)
    public void Delete(int taskId, int engineerId);
    //get list of tasks of a given engineer

    public void Update(int id, DateTime _ScheduledDate);


}
