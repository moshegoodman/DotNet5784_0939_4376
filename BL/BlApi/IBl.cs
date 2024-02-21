

namespace BlApi;

public interface IBl
{
    public IEngineer Engineer { get; }
    public ITask Task { get; }
    public void InitializeDB();
    public void ResetDB();

    #region clock
    public DateTime Clock { get; }

    public void AddOneHour();

    public void AddOneDay();

    public void InitDate();

    #endregion
}
