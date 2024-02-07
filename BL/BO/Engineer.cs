
namespace BO;
public class Engineer
{
    private int engineer_id;
    private object value;
    private EngineerExperience level;

    public Engineer(int engineer_id, string email, double cost, string name, EngineerExperience level, object value)
    {
        this.engineer_id = engineer_id;
        Email = email;
        Cost = cost;
        Name = name;
        this.level = level;
        this.value = value;
    }

    public int Id { get; init; }
    public string Email { get; set; }
    public double Cost { get; set; }
    public string Name { get; set; }
    public DO.EngineerExperience Level { get; set; }
    public BO.TaskInEngineer? Task { get; set; }
}
