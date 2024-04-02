
namespace BO;
/// <summary>
/// </summary>
/// <param name="Id">The engineers id</param>
/// <param name="Email">The engineers email</param>
/// <param name="Cost">Engineers price per hour</param>
/// <param name="Name">Engineers name</param>
/// <param name="Level">Engineers Level</param>
/// <param name="Task">A task that the engineer was designated to do</param>
/// 
/// 

public class Engineer
{
    public int Id { get; init; }
    public string Email { get; set; }
    public double Cost { get; set; }
    public string Name { get; set; }
    public string Picture { get; set; }
    public BO.EngineerExperience Level { get; set; }
    public BO.TaskInEngineer? Task { get; set; }

    //public override string ToString()
    //{
    //    string a = $"Name:\t{Name}\nID:\t{Id}\nemail:\t{Email}\nCost:\t{Cost}\nLevel:\t{Level}\n";
    //    if (Task != null)
    //    {
    //        a = a + $"{Task}\n";
    //    }
    //    return a;
    //}
    public override string ToString() => this.ToStringProperty();
}
