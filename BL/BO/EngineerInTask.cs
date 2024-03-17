namespace BO;
/// <summary>
/// </summary>
/// <param name="Id">The engineers id</param>
/// <param name="Name">Engineers name</param>
/// 
public class EngineerInTask
{

    public int Id { get; init; }
    public string Name { get; set; }
    public override string ToString() => this.ToStringProperty();

}
