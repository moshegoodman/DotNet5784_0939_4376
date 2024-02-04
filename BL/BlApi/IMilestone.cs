

namespace BlApi;
/// <summary>
///     
/// </summary>
public interface IMilestone
{
    public int Create(BO.Milestone item);
    public BO.Milestone? Read(int id);
    public IEnumerable<BO.MilestoneInList> ReadAll();
    public void Update(BO.Milestone item);
    public void Delete(int id);
    //return list of milestones that have to do with an engineer 
    public IEnumerable<BO.Milestone> EngineersMilestones(BO.Engineer engineer);
    //return a list of engineers thjat have to do with a given milestone
    public IEnumerable<BO.Engineer> EnginnersOfAMilstone(BO.Milestone engineer);
}
