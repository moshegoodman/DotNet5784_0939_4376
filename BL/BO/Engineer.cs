namespace BO;
public class Engineer
{
    int Id {  get; init; }
    string Email {  get; set; }
    double Cost {  get; set; }
    string Name {  get; set; }
    DO.EngineerExperience Level {  get; set; }
    BO.TaskInEngineer? Task {  get; set; }
}
