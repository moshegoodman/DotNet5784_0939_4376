

namespace BlApi;
/// <summary>
///     
/// </summary>
/// 
public interface IEngineer
{
    public int Create(BO.Engineer item);//saves engineer to database
    public BO.Engineer? Read(int id);//returns engineer of a given id
    public IEnumerable<BO.Engineer>? ReadAll(Func<BO.Engineer, bool>? filter = null);//returns all engineers
    public void Update(BO.Engineer item);//updates a given engineer
    public void Delete(int id);//deletes a given engineer


    //-----------------------SETTERS----------------------------
    public void SetEmail(int engineerId, string email);
    public void SetName(int engineerId, string name);
    public void SetCost(int engineerId, double cost);
    public void UpdateLevel(int engineerId, BO.EngineerExperience level);

}
