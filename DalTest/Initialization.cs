namespace DalTest;
using DalApi;
using DO;

public static class Initialization
{

    const int MIN_ID = 200000000;
    const int MAX_ID = 400000000;
    const int LOW_SAL = 50;
    const int HIGH_SAL = 300;
    private static IDependency? s_dalDependency; //stage 1
    private static ITask? s_dalTask; //stage 1
    private static IEngineer? s_dalEngineer; //stage 1

    private static readonly Random s_rand = new();

    private static void createEngineer()
    {
        string[] engineerNames =
        {
            "Edward","Boris","Moses","David","Joseph"
        };
        foreach (var _name in engineerNames)
        {
            int _id;
            do
                _id = s_rand.Next(MIN_ID, MAX_ID);
            while (s_dalEngineer!.Read(_id) is not null);

            string _email = $"{_name.ToLower()}@eng.com";

            double _cost = s_rand.NextDouble() * (HIGH_SAL - LOW_SAL) + LOW_SAL;

            DO.EngineerExperience _level = (DO.EngineerExperience)s_rand.Next(4);

            Engineer newEng = new(_id, _email, _cost, _name, _level);

            s_dalEngineer!.Create(newEng);

        }

    }

    private static void createTask()
    {

        for (int i = 0; i < 20; ++i)
        {

            string _alias = $"T{i}";

            string _discription = $"D{i}";

            DateTime _startDate;
            do
                _startDate = s_rand.Next(1000, 1000000);
            while (s_dalTask!.Read(_DependentOnTask) is null && _DependentOnTask < _DependentTask);
            TimeSpan range = TimeSpan.FromDays(365); // Adjust the range as needed
            DateTime _createdAtDate = startDate + TimeSpan.FromDays(s_rand.Next((int)range.TotalDays));

            double _cost = s_rand.NextDouble() * (HIGH_SAL - LOW_SAL) + LOW_SAL;

            DO.EngineerExperience _level = (DO.EngineerExperience)s_rand.Next(3);

            Engineer newEng = new(_id, _email, _cost, _name, _level);

            s_dalEngineer.Create(newEng);

        }

    }







    private static void createDependency()
    {

        for (int i = 0; i < 40; ++i)
        {

            int _DependentTask;
            do
                _DependentTask = s_rand.Next(1000, 1000000);
            while (s_dalTask!.Read(_DependentTask) is null);

            int _DependentOnTask;
            do
                _DependentOnTask = s_rand.Next(1000, 1000000);
            while (s_dalTask!.Read(_DependentOnTask) is null && _DependentOnTask < _DependentTask);

            Dependency newDep = new(0, _DependentTask, _DependentOnTask);

            s_dalDependency!.Create(newDep);

        }

    }
}
