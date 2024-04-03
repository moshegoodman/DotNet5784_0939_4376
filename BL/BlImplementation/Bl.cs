

namespace BlImplementation;
using BlApi;
using System;

internal class Bl : IBl
{

    public IEngineer Engineer => new EngineerImplementation();
    public ITask Task => new TaskImplementation(this);

    public void InitializeDB()
    {
        DalTest.Initialization.Do();
    }
    public void ResetDB()
    {
        DalTest.Initialization.ResetData();

    }

    #region 



    private static DateTime s_Clock = DateTime.Now.Date;
    public DateTime Clock { get { return s_Clock; } private set { s_Clock = value; } }


    void IBl.AddOneDay()
    {
        Clock = Clock.AddDays(1);
    }

    void IBl.AddOneHour()
    {
        Clock = Clock.AddHours(1);
    }

    void IBl.InitDate()
    {
        Clock = DateTime.Now.Date;
    }

    #endregion
}
