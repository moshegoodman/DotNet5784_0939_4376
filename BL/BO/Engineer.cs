
namespace BO;
public class Engineer
{
    public int Id { get; init; }
    public string Email { get; set; }
    public double Cost { get; set; }
    public string Name { get; set; }
    public BO.EngineerExperience Level { get; set; }
    public BO.TaskInEngineer? Task { get; set; }

    public override string ToString()
    {
        string a = $"Name:\t{Name}\nID:\t{Id}\nemail:\t{Email}\nCost:\t{Cost}\nLevel:\t{Level}\n";
        if (Task != null)
        {
            a.Insert(a.Length - 1, $"Task:   {Task}");
        }
        return a;
    }

}
