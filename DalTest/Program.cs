namespace DalTest;
using Dal;
using DalApi;
using DO;

internal class Program
{
    // Main menu to navigate through entities
    private void main_menu()
    {
        int a;
        do
        {
            Console.WriteLine("enter entity to check");
            Console.WriteLine("enter 0 to leave main menu");
            Console.WriteLine("enter 1 to check Tasks ");
            Console.WriteLine("enter 2 to check Dependencies ");
            Console.WriteLine("enter 3 to check Engineers ");
            a = Convert.ToInt32(Console.ReadLine())!;

            switch (a)
            {
                case 0:
                    return;
                case 1:
                    TaskMenu();
                    break;
                case 2:
                    MenuDependents();
                    break;
                case 3:
                    MenuEngineers();
                    break;
                default:
                    Console.WriteLine("enter a number between 0 and 3");
                    break;
            }
        }
        while (a != 0);
    }

    // Method to get user input for a DateTime
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
    // Method to get user input for a TimeSpan
    private TimeSpan? GetTimeSpan()
    {
        Console.WriteLine("Enter how meny days:");
        int days = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter how meny hours:");
        int hours = Convert.ToInt32(Console.ReadLine());
        try
        {
            return new TimeSpan(days, hours, 0);
        }
        catch (Exception)
        {
            Console.WriteLine("Invalid days or hours entered.");
            return null;
        }
    }

    // Task-related menu
    private void TaskMenu()
    {
        do
        {

            Console.WriteLine("Enter 1 to exit the task menu");
            Console.WriteLine("Enter 2 to add a new task ");
            Console.WriteLine("Enter 3 to read a task ");
            Console.WriteLine("Enter 4 to read all tasks ");
            Console.WriteLine("Enter 5 to update a task ");
            Console.WriteLine("Enter 6 to delete a task");
            int a = Convert.ToInt32(Console.ReadLine())!;

            try
            {
                switch (a)
                {
                    case 1:
                        return;
                    case 2:
                        TaskCreate();
                        break;
                    case 3:
                        TaskRead();
                        break;

                    case 4:
                        TaskReadAll();
                        break;
                    case 5:
                        TaskUpdate();
                        break;
                    case 6:
                        TaskDelete();
                        break;
                    default:
                        Console.WriteLine("enter a number between 0 and 6");
                        break;
                }
            }
            catch (Exception err) { Console.WriteLine(err); }
        } while (true);
    }

    // Method to create a new task
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
        ITask? copyTask = new TaskImplementation();
        Console.WriteLine("new Task id:");
        Console.WriteLine(copyTask!.Create(newTask));
    }
    //This dethod prints the fields of a task(the user gives the id)
    private void TaskRead()
    {
        Console.WriteLine("Enter the Task id:");
        int _taskId = Convert.ToInt32(Console.ReadLine())!;

        ITask? readTask = new TaskImplementation();

        Console.WriteLine(readTask!.Read(_taskId));
    }
    // The method prints the fields of all tasks
    private void TaskReadAll()
    {
        ITask? readTask = new TaskImplementation();
        List<DO.Task> newList = readTask!.ReadAll();
        foreach (DO.Task task in newList) { Console.WriteLine(task); }
    }
    //The method updates the task (the user gives the id then gives the values for the fields)
    private void TaskUpdate()
    {
        Console.WriteLine("Enter the id of the task:");
        int _id = Convert.ToInt32(Console.ReadLine()!);

        ITask? readTask = new TaskImplementation();
        if (readTask!.Read(_id) == null)
            return;
        Console.WriteLine(readTask!.Read(_id));


        Console.WriteLine("Enter the new alias of the task:");
        string _alias = Console.ReadLine()!;

        Console.WriteLine("Enter a description of the task:");
        string _description = Console.ReadLine()!;

        Console.WriteLine("Enter the new create-date of the task by following these steps!");
        DateTime? _createdAtDate = GetDateTime();

        Console.WriteLine("Enter the new complexity of the task 1-5:");
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
        ITask? copyTask = new TaskImplementation();
        copyTask!.Update(newTask);
    }
    //The method deletes a task given by the user(the user enters the id)
    private void TaskDelete()
    {
        Console.WriteLine("Enter the task id to delete:");
        int _taskId = Convert.ToInt32(Console.ReadLine());
        ITask? _task = new TaskImplementation();
        _task!.Delete(_taskId);
    }

    // Dependency-related menu

    private void MenuDependents()
    {
        do
        {
            Console.WriteLine("Enter 1 to exit the dependency menu");
            Console.WriteLine("Enter 2 to add a new dependency ");
            Console.WriteLine("Enter 3 to read a dependency ");
            Console.WriteLine("Enter 4 to read all dependencies ");
            Console.WriteLine("Enter 5 to update a dependency ");
            Console.WriteLine("Enter 6 to delete a dependency");
            int a = Convert.ToInt32(Console.ReadLine())!;

            try
            {
                switch (a)
                {
                    case 2:
                        DependencyCreate();
                        break;
                    case 3:
                        DependencyRead();
                        break;

                    case 4:
                        DependencyReadAll();
                        break;
                    case 5:
                        DependencyUpdate();
                        break;
                    case 6:
                        DependencyDelete();
                        break;
                    case 1:
                        return;
                    default:
                        Console.WriteLine("enter a number between 0 and 6");
                        break;
                }
            }
            catch (Exception err) { Console.WriteLine(err); }
        }
        while (true);
    }

    //The method creates a new dependency(the user enters all the field values)
    private void DependencyCreate()
    {
        Console.WriteLine("Enter the dependant task:");
        int dependant_task = Convert.ToInt32(Console.ReadLine())!;

        Console.WriteLine("Enter the dependamt on tasktask:");
        int dependamt_on_tasktask = Convert.ToInt32(Console.ReadLine())!;


        DO.Dependency newDependency = new(0, dependant_task, dependamt_on_tasktask);
        IDependency? copyDependency = new DependencyImplementation();
        Console.WriteLine("new dependency id:");
        Console.WriteLine(copyDependency.Create(newDependency));
    }
    // prints all fields of a given dependency (the user enters the id)
    private void DependencyRead()
    {
        Console.WriteLine("Enter the dependancy id:");
        int _dependancy = Convert.ToInt32(Console.ReadLine())!;

        IDependency? readDependency = new DependencyImplementation();

        Console.WriteLine(readDependency!.Read(_dependancy));
    }
    // prints all fields of all dependencies
    private void DependencyReadAll()
    {
        IDependency? readDependency = new DependencyImplementation();
        List<DO.Dependency> newList = readDependency!.ReadAll();
        foreach (DO.Dependency dependency in newList) { Console.WriteLine(dependency); }
    }
    //this method updates a given dependency
    private void DependencyUpdate()
    {
        IDependency? readDependency = new DependencyImplementation();

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

        readDependency!.Update(updatedDependency);
    }
    //this metod deletes a given dependency
    private void DependencyDelete()
    {
        Console.WriteLine("Enter the dependancy id:");
        int _dependancy = Convert.ToInt32(Console.ReadLine())!;
        IDependency? _dependency = new DependencyImplementation();
        _dependency!.Delete(_dependancy);
    }




    // Dependency-related menu
    private void MenuEngineers()
    {
        do
        {
            Console.WriteLine("Enter 1 to exit the engineers menu");
            Console.WriteLine("Enter 2 to add an new engineer ");
            Console.WriteLine("Enter 3 to read an engineer ");
            Console.WriteLine("Enter 4 to read all engineers ");
            Console.WriteLine("Enter 5 to update an engineer ");
            Console.WriteLine("Enter 6 to delete an engineer");
            int a = Convert.ToInt32(Console.ReadLine())!;

            try
            {
                switch (a)
                {
                    case 1:
                        return;
                    case 2:
                        EngineerCreate();
                        break;
                    case 3:
                        EngineerRead();
                        break;

                    case 4:
                        EngineerReadAll();
                        break;
                    case 5:
                        EngineerUpdate();
                        break;
                    case 6:
                        EngineerDelete();
                        break;
                    default:
                        Console.WriteLine("enter a number between 0 and 6");
                        break;
                }
            }
            catch (Exception err) { Console.WriteLine(err); }
        }
        while (true);
    }

    //The method creates a new engineer (the user enters all the field values)
    private void EngineerCreate()
    {
        Console.WriteLine("Enter engineers id:");
        int _engineer_id = Convert.ToInt32(Console.ReadLine())!;

        Console.WriteLine("Enter engineers email:");
        string Email = Console.ReadLine()!;

        Console.WriteLine("Enter engineers price per hour:");
        double Cost = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine("Enter engineers name:");
        string Name = Console.ReadLine()!;

        Console.WriteLine("Enter engineers level:");
        DO.EngineerExperience Level = (DO.EngineerExperience)Convert.ToInt32(Console.ReadLine());

        DO.Engineer newEngineer = new(_engineer_id, Email, Cost, Name, Level);
        IEngineer? copyEngineer = new EngineerImplementation();
        Console.WriteLine("new enginerr id:");
        Console.WriteLine(copyEngineer!.Create(newEngineer));
    }
    // prints all fields of a given engineer (the user enters the id)
    private void EngineerRead()
    {
        Console.WriteLine("Enter the dependancy id:");
        int _engineer = Convert.ToInt32(Console.ReadLine())!;
        IEngineer? readEngineer = new EngineerImplementation();

        Console.WriteLine(readEngineer!.Read(_engineer));
    }
    // prints all fields of all engineers
    private void EngineerReadAll()
    {
        IEngineer? readEngineer = new EngineerImplementation();
        List<DO.Engineer> newList = readEngineer!.ReadAll();
        foreach (DO.Engineer engineer in newList) { Console.WriteLine(engineer); }
    }
    private void EngineerUpdate()
    {
        IEngineer? readEngineer = new EngineerImplementation();

        Console.WriteLine("Enter engineers id:");
        int _engineerId = Convert.ToInt32(Console.ReadLine())!;


        if (readEngineer!.Read(_engineerId) == null)
            return;

        Console.WriteLine(readEngineer!.Read(_engineerId));


        Console.WriteLine("Enter engineers email:");
        string Email = Console.ReadLine()!;

        Console.WriteLine("Enter engineers price per hour:");
        double? Cost = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine("Enter engineers name:");
        string Name = Console.ReadLine()!;

        Console.WriteLine("Enter engineers level:");
        DO.EngineerExperience? Level = (DO.EngineerExperience)Convert.ToInt32(Console.ReadLine());


        if (Email is null || Cost is null || Name is null || Level is null)
            return;
        DO.Engineer newEngineer = new(_engineerId, Email, Cost!.Value, Name, Level!.Value);

        readEngineer!.Update(newEngineer);
    }
    //this metod deletes a given engineer
    private void EngineerDelete()
    {
        Console.WriteLine("Enter the engineer id to delete:");
        int _engineer = Convert.ToInt32(Console.ReadLine())!;
        IEngineer? readEngineer = new EngineerImplementation();
        readEngineer!.Delete(_engineer);
    }





    private static ITask? s_dalTask = new TaskImplementation();
    private static IDependency? s_dalDependency = new DependencyImplementation();
    private static IEngineer? s_dalEngineer = new EngineerImplementation();

    //Main method
    public static void Main(string[] args)
    {
        try
        {
            Initialization.Do(s_dalTask, s_dalDependency, s_dalEngineer);
            Program a = new();
            a.main_menu();
            Console.WriteLine("main menu complete");
        }
        catch (Exception err) { Console.WriteLine(err); }

    }
}



