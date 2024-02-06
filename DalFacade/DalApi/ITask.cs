namespace DalApi;
using DO;
public interface ITask : ICrud<Task>
{

    public DateTime? getStartDate();
    public DateTime? setStartDate();
    public DateTime? getStatus();
    public DateTime? setStatus();
}
