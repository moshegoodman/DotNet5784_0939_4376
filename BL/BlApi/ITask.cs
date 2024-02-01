

namespace BlApi;
/// <summary>
///     
/// </summary>
public interface ITask
{
    public int Create(BO.Task item);
    public BO.Task? Read(int id);
    public IEnumerable<BO.TaskInList> ReadAll();
    public void Update(BO.Task item);
    //uses update designate an engineer to the task
    //define the project schedule and produce milestones
    //engineer update only progress(status)
    public void Delete(int id);
    //get list of tasks of a given engineer


}
