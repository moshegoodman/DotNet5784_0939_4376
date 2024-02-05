namespace BO;

public class TaskInEngineer
{
    public TaskInEngineer(int id, string alias)
    {
        Id = id;
        this.alias = alias;
    }

    public int Id { get; init; }
    public string alias {  get; set; }
}
