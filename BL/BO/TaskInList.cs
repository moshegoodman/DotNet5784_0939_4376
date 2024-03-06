namespace BO;

/// <summary>
/// 
/// </summary>
/// 
/// <param name="Id">A unique number for each task</param>
/// <param name="Alias">An optional alias or alternate identifier for the task</param>
/// <param name="Description">A description providing details about the task</param>
/// <param name="Status">The date and time when the task was created</param>

public class TaskInList
{
    public int Id { get; init; }
    public string Description { get; set; }
    public string Alias { get; set; }
    public BO.Status Status { get; set; }


    public override string ToString()

    {
        string a = $"ID:\t{Id}\nAlias:\t{Alias}\nDescription:\t{Description}\nStatus:\t{Status}\n";

        return a;
    }
}
