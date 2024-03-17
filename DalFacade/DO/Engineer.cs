namespace DO;
/// <summary>
/// 
/// </summary>
/// <param name="Id">A unique number for each engineer</param>
/// <param name="Email">the engineers email</param>
/// <param name="Cost">the engineers salary per hour</param>
/// <param name="Name">engineers full name</param>
/// <param name="Level">engineers level</param>
public record Engineer
(
    int Id,
    string Email = "",
    double Cost = 0,
    string Name = "",
    string Picture = "",
    DO.EngineerExperience Level = EngineerExperience.Beginner
)
{
    Engineer() : this(0) { }

}
