namespace BO;
public class TaskInList
{
    public int Id { get; init; }
    public string Description { get; set; }
    public string Alias { get; set; }
    public BO.Status Status { get; set; }


    public override string ToString()
    {
        string a = $"\tID:\t{Id}\n\tAlias:\t{Alias}\n\tDescription:\t{Description}\n\tStatus:\t{Status}\n";

        return a;
    }
}
