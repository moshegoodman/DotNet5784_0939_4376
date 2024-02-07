namespace BO;
public class TaskInList
{
    public int Id { get; init; }
    public string Description { get; set; }
    public string Alias { get; set; }
    public BO.Status Status { get; set; }


    public override string ToString()
    {
        string a = $"Name:\nID:\t{Id}\nemail:\t\nCost:\t\nLevel:\t\n";
        //if (Task != null)
        //{
        //    a.Insert(a.Length, $"Task:   {Task}");
        //}
        return a;
    }
}
