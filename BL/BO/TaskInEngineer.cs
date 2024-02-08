namespace BO;

public class TaskInEngineer
{
    public int Id { get; init; }
    public string alias { get; set; }

    public override string ToString()
    {
        string a = $"\tTask asigned to engineer:\n\tID:\t{Id}\n\tAlias:\t{alias}\n\n";

        return a;
    }
}
