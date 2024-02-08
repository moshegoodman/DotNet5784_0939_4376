

namespace BlApi;
/// <summary>
///     
/// </summary>
/// 
public interface IEngineer
{
    public int Create(BO.Engineer item);
    public BO.Engineer? Read(int id);
    public IEnumerable<BO.Engineer>? ReadAll(Func<BO.Engineer, bool>? filter = null);
    public void Update(BO.Engineer item);
    public void Delete(int id);
    // public BO.EngineerInTask GetDetailedCourseForStudent(int StudentId, int CourseId);


    //-----------------------SETTERS----------------------------
    public void SetEmail(int engineerId, string email);
    public void SetName(int engineerId, string name);
    public void SetCost(int engineerId, double cost);
    public void UpdateLevel(int engineerId, BO.EngineerExperience level);

}
