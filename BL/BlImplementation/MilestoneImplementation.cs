namespace BlImplementation;
using BlApi;

internal class MilestoneImplementation : IMilestone
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Milestone item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.Milestone> EngineersMilestones(BO.Engineer engineer)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.Engineer> EnginnersOfAMilstone(BO.Milestone engineer)
    {
        throw new NotImplementedException();
    }

    public BO.Milestone? Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.MilestoneInList> ReadAll()
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Milestone item)
    {
        throw new NotImplementedException();
    }
}
