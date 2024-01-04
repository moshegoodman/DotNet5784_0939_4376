namespace DO;
/// <summary>
/// Dependency Entity says what task is dependent on which previous task
/// </summary>
/// <param name="_id">A unique number for each occurance</param>
/// <param name="DependentTask">the id of the dependent task</param>
/// <param name="DependsOnTask">The id of the predecessor task</param>
public record Dependency
(
    int? Id,
    int? DependentTask,
    int? DependsOnTask
)
{
    Dependency() : this(null, null, null) { }
    //public Dependency(int _id, int _dependentTask, int _dependsOnTask)
    //{

    //    Id = _id;
    //    DependentTask = _dependentTask;
    //    DependsOnTask = _dependsOnTask;
        
    //}

}
