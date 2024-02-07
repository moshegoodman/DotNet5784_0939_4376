
namespace BO;
public class Engineer
{
    public int Id { get; init; }
    public string Email { get; set; }
    public double Cost { get; set; }
    public string Name { get; set; }
    public BO.EngineerExperience Level { get; set; }
    public BO.TaskInEngineer? Task { get; set; }
}
