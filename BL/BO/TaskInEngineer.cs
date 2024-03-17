namespace BO;
/// <summary>
/// 
/// </summary>
/// 
/// <param name="Id">A unique number for each task</param>
/// <param name="Alias">An optional alias or alternate identifier for the task</param>

public class TaskInEngineer
{
    public int Id { get; init; }
    public string alias { get; set; }

    public override string ToString() => this.ToStringProperty();

}
