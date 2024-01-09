namespace DalTest;
using DalApi;
using DO;




//creates a bunch of tasks engineers dependencies randomly
public static class Initialization
{
    // Constants for setting range limits and salary boundaries
    const int MIN_ID = 200000000;
    const int MAX_ID = 400000000;
    const int LOW_SAL = 50;
    const int HIGH_SAL = 300;


    private static IDependency? s_dalDependency;
    private static ITask? s_dalTask;
    private static IEngineer? s_dalEngineer;

    private static readonly Random s_rand = new();

    // Method to create engineers with random attributes

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

    // Method to create tasks with random attributes
    private static void createTask()
    {

        for (int i = 0; i < 20; ++i)
        {

            string _alias = $"T{i}";

            string _discription = $"D{i}";

            DateTime _createdAtDate = DateTime.Now.AddDays(-s_rand.Next(365));


            DO.EngineerExperience _complexity = (DO.EngineerExperience)s_rand.Next(4);

            Task newTask = new(i, _alias, _discription, _createdAtDate, _complexity);

            s_dalTask!.Create(newTask);

        }

    }


    // Method to create dependencies between tasks
    private static void createDependency()
    {

        for (int i = 0; i < 40; ++i)
        {

            int _dependentTask;
            do
                _dependentTask = s_rand.Next(1000, 2000);
            while (s_dalTask!.Read(_dependentTask) is null);

            int _dependentOnTask;
            do
                _dependentOnTask = s_rand.Next(1000, 2000);
            while (s_dalTask!.Read(_dependentOnTask) is null || _dependentOnTask >= _dependentTask);

            Dependency newDep = new(i, _dependentTask, _dependentOnTask);

            s_dalDependency!.Create(newDep);

        }
    }


    // Main method to initiate the data creation process
    public static void Do(ITask? dalTask, IDependency? dalDependency, IEngineer? dalEngineer)
    {
        s_dalTask = dalTask ?? throw new NullReferenceException("DAL can not be null!");
        s_dalDependency = dalDependency ?? throw new NullReferenceException("DAL can not be null!");
        s_dalEngineer = dalEngineer ?? throw new NullReferenceException("DAL can not be null!");

        createTask();
        createDependency();
        createEngineer();

    }
}
