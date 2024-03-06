namespace DalApi;
using DO;
public interface ITask : ICrud<Task>
{

    public DateTime? GetStartDate();
    public void SetStartDate(DateTime startDate);
    public int? GetStatus();
    public void IncreaseStatus();
    public void NullifyStatus();

}
