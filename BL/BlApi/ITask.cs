

namespace BlApi;
/// <summary>
///     
/// </summary>
public interface ITask
{
    public int Create(BO.Task boTask);
    public BO.Task? Read(int TaskId);
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool>? filter = null);
    public void Update(BO.Task boTask);
    //uses update designate an engineer to the task
    public void UpdateStage3(BO.Task boTask);
    public void DesignateEngineer(int idTask, int engineerId);

    //define the project schedule and produce milestones
    //engineer update only progress(status)
    public void Delete(int TaskId);
    //get list of tasks of a given engineer

    public void Update(int TaskId, DateTime _ScheduledDate);
    public void SetStartDate(int taskId, DateTime? _startDate = null);

    public void SetCompleteDate(int taskId, DateTime? _completeDate = null);







    //------------------------------------------
    public void SetStage1();
    public void SetStage2(DateTime startDate);
    public void SetStage3();



}
