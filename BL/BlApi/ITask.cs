﻿

namespace BlApi;
/// <summary>
///     
/// </summary>
public interface ITask
{
    public int Create(BO.Task item);
    public BO.Task? Read(int id);
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool>? filter = null);
    public void Update(BO.Task item, DateTime? _scheduledDate = null);
    //uses update designate an engineer to the task
    public void DesignateEngineer(BO.Task item);

    //define the project schedule and produce milestones
    //engineer update only progress(status)
    public void Delete(int id);
    //get list of tasks of a given engineer


}
