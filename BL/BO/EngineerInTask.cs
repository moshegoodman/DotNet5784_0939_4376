namespace BO;
/// <summary>
/// </summary>
/// <param name="Id">The engineers id</param>
/// <param name="Name">Engineers name</param>
/// 
public class EngineerInTask
{

    public int Id { get; init; }
    public string Name { get; set; }
    public override string ToString()
    {
        string a = $"\tTask asigned to engineer:\n\tID:\t{Id}\nName:\t{Name}\n\n";

        return a;
    }
}
