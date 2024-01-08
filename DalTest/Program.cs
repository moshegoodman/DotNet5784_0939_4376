namespace DalTest;
using Dal;
using DalApi;
using DO;

internal class Program
{

    private DateTime? GetDateTime()
    {
        Console.WriteLine("Enter the year:");
        int year = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter the month:");
        int month = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter th day:");
        int day = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter the hour:");
        int hour = Convert.ToInt32(Console.ReadLine());
        try
        {
            return new DateTime(year, month, day, hour, 0, 0);
        }
        catch (Exception)
        {
            Console.WriteLine("Invalid date or time entered.");
            return null;
        }
    }
    private void TaskMenu()
    {
        Console.WriteLine("Enter the alias of the task:");
        string _alias = Console.ReadLine()!;
        Console.WriteLine("Enter 1 to exit the task menu");
        Console.WriteLine("Enter 2 to add a new task ");
        Console.WriteLine("Enter 3 to read a task ");
        Console.WriteLine("Enter 4 to read all tasks ");
        Console.WriteLine("Enter 5 to update a task ");
        Console.WriteLine("Enter 6 to delete a task");
        string a = Console.ReadLine()!;
        do
            switch (a)
            {
                case "2":
                    TaskCreate();
                    break;
                case "3":
                    TaskRead();
                    break;

                case "4":
                    TaskReadAll();
                    break;
                case "5":
                    TaskUpdate();
                    break;
                case "6":
                    TaskDelete();
                    break;

            }
        while (a != "1");
    }

    private void TaskCreate()
    {
        Console.WriteLine("Enter the alias of the task:");
        string _alias = Console.ReadLine()!;

        Console.WriteLine("Enter a description of the task:");
        string _description = Console.ReadLine()!;

        Console.WriteLine("Enter the date of the task by following these steps!");
        DateTime? _createdAtDate = GetDateTime();

        Console.WriteLine("Enter the complexity of the task 1-5:");
        DO.EngineerExperience _taskComplexity = (DO.EngineerExperience)Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Enter the deliverables of the task:");
        string _deliverables = Console.ReadLine()!;

        Console.WriteLine("Enter the remark of the task:");
        string _remark = Console.ReadLine()!;

        Console.WriteLine("Enter if the task is milestone:  Y/N");
        bool isMilestone = (Console.ReadLine()! == "Y");

        Console.WriteLine("Enter how meny time it will take, by following these steps!");
        TimeSpan? requiredEffortTime = GetTimeSpan();

        Console.WriteLine("Enter the end-date of the task by following these steps!");
        DateTime? startDate = GetDateTime();

        Console.WriteLine("Enter the start-date of the task by following these steps!");
        DateTime? scheduledDate = GetDateTime();

        Console.WriteLine("Enter the dead-line date of the task by following these steps!");
        DateTime? deadlineDate = GetDateTime();

        Console.WriteLine("Enter the real end-date of the task by following these steps!");
        DateTime? completeDate = GetDateTime();

        Console.WriteLine("Enter the engineer's id of the task:");
        int engineerId = Convert.ToInt32(Console.ReadLine());



        DO.Task newTask = new(0, _alias, _description, _createdAtDate, _taskComplexity, _deliverables, _remark, isMilestone, requiredEffortTime, startDate, scheduledDate, deadlineDate, completeDate, engineerId);
        ITask? copyTask = null;
        copyTask.Create(newTask);
    }
    private void TaskRead()
    {

    }
    private void TaskReadAll()
    {

    }
    private void TaskUpdate()
    {

    }
    private void TaskDelete()
    {

    }








    private void MenuDependents()
    {
        Console.WriteLine("Enter 1 to exit the dependency menu");
        Console.WriteLine("Enter 2 to add a new dependency ");
        Console.WriteLine("Enter 3 to read a dependency ");
        Console.WriteLine("Enter 4 to read all dependencies ");
        Console.WriteLine("Enter 5 to update a dependency ");
        Console.WriteLine("Enter 6 to delete a dependency");
        string a = Console.ReadLine()!;
        do
            switch (a)
            {
                case "2":
                    DependencyCreate();
                    break;
                case "3":
                    DependencyRead();
                    break;

                case "4":
                    DependencyReadAll();
                    break;
                case "5":
                    DependencyUpdate();
                    break;
                case "6":
                    DependencyDelete();
                    break;

            }
        while (a != "1");
    }


    private void DependencyCreate()
    {
        Console.WriteLine("Enter the dependant task:");
        int dependant_task = Convert.ToInt32(Console.ReadLine())!;

        Console.WriteLine("Enter the dependamt on tasktask:");
        int dependamt_on_tasktask = Convert.ToInt32(Console.ReadLine())!;


        DO.Dependency newDependency = new(0, dependant_task, dependamt_on_tasktask);
        DependencyImplementation? copyDependency = null;
        copyDependency!.Create(newDependency);
    }
    private void DependencyRead()
    {
        Console.WriteLine("Enter the dependancy id:");
        int _dependancy = Convert.ToInt32(Console.ReadLine())!;

        DependencyImplementation? readDependency = null;

        Console.WriteLine(readDependency!.Read(_dependancy));
    }
    private void DependencyReadAll()
    {
        List<DO.Dependency>? _allDependencies = null;
        DependencyImplementation? readDependency = null;

        _allDependencies = readDependency!.ReadAll();
        foreach (DO.Dependency dependency in _allDependencies)
        {
            Console.WriteLine(dependency);
        }
    }
    private void DependencyUpdate()
    {
        DependencyImplementation? readDependency = null;

        Console.WriteLine("Enter the dependancy id:");
        int _dependancyId = Convert.ToInt32(Console.ReadLine())!;


        if (readDependency!.Read(_dependancyId) == null)
            return;

        Console.WriteLine(readDependency!.Read(_dependancyId));


        Console.WriteLine("Enter the dependant task:");
        int? dependant_task = Convert.ToInt32(Console.ReadLine())!;
        Console.WriteLine("Enter the dependamt on tasktask:");
        int? dependamt_on_tasktask = Convert.ToInt32(Console.ReadLine())!;



        Dependency updatedDependency = new Dependency(_dependancyId, dependant_task, dependamt_on_tasktask);

        if (dependant_task is null || dependamt_on_tasktask is null)
            return;

        readDependency!.Delete(_dependancyId);
        readDependency!.Create(updatedDependency);
    }
    private void DependencyDelete()
    {
        Console.WriteLine("Enter the dependancy id:");
        int _dependancy = Convert.ToInt32(Console.ReadLine())!;
        IDependency? _dependency = null;
        _dependency!.Delete(_dependancy);
    }















    private void MenuEngineers()
    {
        Console.WriteLine("Enter 1 to exit the engineers menu");
        Console.WriteLine("Enter 2 to add an new engineer ");
        Console.WriteLine("Enter 3 to read an engineer ");
        Console.WriteLine("Enter 4 to read all engineers ");
        Console.WriteLine("Enter 5 to update an engineer ");
        Console.WriteLine("Enter 6 to delete an engineer");
        string a = Console.ReadLine()!;
        do
            switch (a)
            {
                case "2":
                    EngineerCreate();
                    break;
                case "3":
                    EngineerRead();
                    break;

                case "4":
                    EngineerReadAll();
                    break;
                case "5":
                    EngineerUpdate();
                    break;
                case "6":
                    EngineerDelete();
                    break;

            }
        while (a != "1");
    }




    private void EngineerCreate()
    {
        Console.WriteLine("Enter the dependant task:");
        int dependant_task = Convert.ToInt32(Console.ReadLine())!;

        Console.WriteLine("Enter the dependamt on tasktask:");
        int dependamt_on_tasktask = Convert.ToInt32(Console.ReadLine())!;


        int Id;
        string Email = "";
        double Cost = 0;
        string Name = "";
        DO.EngineerExperience Level = EngineerExperience.Beginner;

        DO.Dependency newDependency = new(0, dependant_task, dependamt_on_tasktask);
        DependencyImplementation? copyDependency = null;
        copyDependency!.Create(newDependency);
    }
    private void EngineerRead()
    {
        Console.WriteLine("Enter the dependancy id:");
        int _dependancy = Convert.ToInt32(Console.ReadLine())!;

        DependencyImplementation? readDependency = null;

        Console.WriteLine(readDependency!.Read(_dependancy));
    }
    private void EngineerReadAll()
    {
        List<DO.Dependency>? _allDependencies = null;
        DependencyImplementation? readDependency = null;

        _allDependencies = readDependency!.ReadAll();
        foreach (DO.Dependency dependency in _allDependencies)
        {
            Console.WriteLine(dependency);
        }
    }
    private void EngineerUpdate()
    {
        DependencyImplementation? readDependency = null;

        Console.WriteLine("Enter the dependancy id:");
        int _dependancyId = Convert.ToInt32(Console.ReadLine())!;


        if (readDependency!.Read(_dependancyId) == null)
            return;

        Console.WriteLine(readDependency!.Read(_dependancyId));


        Console.WriteLine("Enter the dependant task:");
        int? dependant_task = Convert.ToInt32(Console.ReadLine())!;
        Console.WriteLine("Enter the dependamt on tasktask:");
        int? dependamt_on_tasktask = Convert.ToInt32(Console.ReadLine())!;



        Dependency updatedDependency = new Dependency(_dependancyId, dependant_task, dependamt_on_tasktask);

        if (dependant_task is null || dependamt_on_tasktask is null)
            return;

        readDependency!.Delete(_dependancyId);
        readDependency!.Create(updatedDependency);
    }
    private void EngineerDelete()
    {
        Console.WriteLine("Enter the dependancy id:");
        int _dependancy = Convert.ToInt32(Console.ReadLine())!;
        IDependency? _dependency = null;
        _dependency!.Delete(_dependancy);
    }





    private static ITask? s_dalTask = new TaskImplementation();
    private static IDependency? s_dalDependency = new DependencyImplementation();
    private static IEngineer? s_dalLinks = new EngineerImplementation();

    public
    static void Main(string[] args)
    {


        try
        {
            Initialization.Do(s_dalTask, s_dalDependency, s_dalLinks);

        }
        catch (Exception a) { Console.WriteLine(a); }
    }

    private void main_menu()
    {
        Console.WriteLine("enter entity to check");
        Console.WriteLine("enter 0 to leave main menu");
        Console.WriteLine("enter 1 to check Tasks ");
        Console.WriteLine("enter 2 to check Dependencies ");
        Console.WriteLine("enter 3 to check Engineers ");
        string a = Console.ReadLine()!;

        do
            switch (a)
            {
                case "1": MenuTasks();
                case "2": MenuDependents();
                case "3": MenuEngineers();
            }
        while (a != "0");
    }

}



