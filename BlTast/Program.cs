using BlApi;
using BlImplementation;
namespace BlTest;


internal class Program
{
    static readonly IBl s_bl = Factory.Get();

    readonly string s_engineers_xml = "engineers";
    static string s_data_config_xml = "data-config";



    //static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    private static void MainMenu()
    {
        int a;
        do
        {
            Console.WriteLine("enter entity to check");
            Console.WriteLine("enter 0 to leave main menu");
            Console.WriteLine("enter 1 to check Tasks ");
            Console.WriteLine("enter 2 to check Engineers ");
            Console.WriteLine("enter 3 to check first stage ");
            Console.WriteLine("enter 4 to check second stage ");
            Console.WriteLine("enter 5 to check third stage ");
            a = Convert.ToInt32(Console.ReadLine())!;
            switch (a)
            {
                case 0:
                    return;
                case 1:
                    TaskMenu();
                    break;
                case 2:
                    EngineerMenu();
                    break;
                case 3:
                    FirstStage();
                    break;
                case 4:
                    SecondStage();
                    break;
                case 5:
                    ThirdStage();
                    break;
                default:
                    Console.WriteLine("enter a number between 0 and 3");
                    break;
            }
        }
        while (a != 0);
    }

    #region task
    private static void TaskMenu()
    {
        int a;
        a = Convert.ToInt32(Console.ReadLine())!;

        do
        {
            Console.WriteLine("Enter 1 to exit the task menu");
            Console.WriteLine("Enter 2 to add a new task ");//for first stage
            Console.WriteLine("Enter 3 to delete a task ");
            Console.WriteLine("Enter 4 to designate an engineer ");
            Console.WriteLine("Enter 5 to read a task ");
            Console.WriteLine("Enter 6 to read all tasks");
            Console.WriteLine("Enter 7 to update a task");
            Console.WriteLine("Enter 8 to update a task scheduled date");
            Console.WriteLine("Enter 9 to Get 'Engineer In Task'  ");
            Console.WriteLine("Enter 10 to get list of dependent tasks");
            Console.WriteLine("Enter 11 to get  task's status");
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
                        TaskDelete();
                        break;
                    case 4:
                        TaskDesignateEngineer();
                        break;

                    case 5:
                        TaskRead();
                        break;
                    case 6:
                        TaskReadAll();
                        break;
                    case 7:
                        TaskUpdate();//might wanna delete
                        break;
                    case 8:
                        TaskUpdateSchuledDate();
                        break;
                    case 9:
                        GetEngineerInTask();//might wanna delete it
                        break;
                    case 10:
                        TaskGetDependencies();
                        break;
                    case 11:
                        TaskGetStatus();
                        break;
                    default:
                        Console.WriteLine("enter a number between 0 and 6");
                        break;
                }
            }
            catch (Exception err) { Console.WriteLine(err.Message); }
        } while (a != 1);
    }
    private static void TaskCreate()
    {

        string? _remarks = null;
        string answer = "n";

        Console.WriteLine("enter all the tasks that are necessery for the project");

        Console.WriteLine("enter task's  alias, description, required effort time and complexity ");
        string _alias = Console.ReadLine();
        string _description = Console.ReadLine();
        TimeSpan _requiredEffortTime = ProjectImplementation.GetTimeSpanFromUser();
        BO.EngineerExperience _complexity = (BO.EngineerExperience)int.Parse(Console.ReadLine());


        Console.WriteLine("add remarks to the task?");
        answer = Console.ReadLine();
        if (answer == "y" || answer == "Y")
        {
            Console.WriteLine("enter description");
            _remarks = Console.ReadLine();

        }
        Console.WriteLine("add a dependency to the task?");
        answer = Console.ReadLine();
        List<BO.TaskInList> tasks = new List<BO.TaskInList>();
        while (answer == "y" || answer == "Y")
        {
            Console.WriteLine("enter the id of the task that it depends on");
            int _taskId = int.Parse(Console.ReadLine());
            BO.TaskInList _dependentTask = BO.Tools.GetTaskInList(_taskId);
            tasks.Add(_dependentTask);

            Console.WriteLine("add another dependency to the task?");
            answer = Console.ReadLine();

        }

        BO.Task boTask = new BO.Task()
        {
            Id = 0,
            Alias = _alias,
            Description = _description,
            CreatedAtDate = DateTime.Now,
            Status = BO.Status.Unscheduled,
            Dependencies = tasks,
            Milestone = null,
            Complexity = _complexity,
            Deliverables = null,
            Remarks = _remarks,
            RequiredEffortTime = _requiredEffortTime,
            StartDate = null,
            ScheduledDate = null,
            ForecastDate = null,
            DeadlineDate = null,
            CompleteDate = null,
            Engineer = null
        };
        s_bl.Task.Create(boTask);
    }
    //This method prints the fields of a task(the user gives the id)
    //private static void TaskDesignateEngineer();
    private static void TaskRead()
    {
        Console.WriteLine("Enter the Task id:");
        int _taskId = Convert.ToInt32(Console.ReadLine())!;



        Console.WriteLine(s_bl!.Task.Read(_taskId));
    }
    // The method prints the fields of all tasks
    private static void TaskReadAll()
    {
        IEnumerable<BO.TaskInList> newList = s_bl.Task.ReadAll();
        foreach (BO.TaskInList task in newList) { Console.WriteLine(task); }
    }
    //The method updates the task (the user gives the id then gives the values for the fields)
    private static void TaskUpdate()
    {
        Console.WriteLine("Enter the id of the task:");
        int _id = Convert.ToInt32(Console.ReadLine()!);

        if (s_bl!.Task.Read(_id) == null)
            return;
        Console.WriteLine(s_bl!.Task.Read(_id));


        Console.WriteLine("Enter the new alias of the task:");
        string _alias = Console.ReadLine()!;

        Console.WriteLine("Enter a description of the task:");
        string _description = Console.ReadLine()!;

        Console.WriteLine("Enter the new create-date of the task by following these steps!");
        DateTime? _createdAtDate = ProjectImplementation.GetDateTimeFromUser();

        Console.WriteLine("Enter the new complexity of the task 1-5:");
        DO.EngineerExperience _taskComplexity = (DO.EngineerExperience)Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Enter the deliverables of the task:");
        string _deliverables = Console.ReadLine()!;

        Console.WriteLine("Enter the remark of the task:");
        string _remark = Console.ReadLine()!;

        Console.WriteLine("Enter if the task is milestone:  Y/N");
        bool isMilestone = (Console.ReadLine()! == "Y");

        Console.WriteLine("Enter how many time it will take, by following these steps!");
        TimeSpan? requiredEffortTime = ProjectImplementation.GetTimeSpanFromUser();

        Console.WriteLine("Enter the end-date of the task by following these steps!");
        DateTime? startDate = ProjectImplementation.GetDateTimeFromUser();

        Console.WriteLine("Enter the start-date of the task by following these steps!");
        DateTime? scheduledDate = ProjectImplementation.GetDateTimeFromUser();

        Console.WriteLine("Enter the dead-line date of the task by following these steps!");
        DateTime? deadlineDate = ProjectImplementation.GetDateTimeFromUser();

        Console.WriteLine("Enter the real end-date of the task by following these steps!");
        DateTime? completeDate = ProjectImplementation.GetDateTimeFromUser();

        Console.WriteLine("Enter the engineer's id of the task:");
        int engineerId = Convert.ToInt32(Console.ReadLine());



        DO.Task newTask = new(_id, _alias, _description, (System.DateTime)_createdAtDate, _taskComplexity, _deliverables, _remark, isMilestone, requiredEffortTime, startDate, scheduledDate, deadlineDate, completeDate, engineerId);
        s_bl!.Task.Update(newTask);
    }//might wanna delete it

    private static void TaskDesignateEngineer()
    {
        Console.WriteLine("enter task id:");
        int taskId = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("enter engineer's id:");
        int engineerId = Convert.ToInt32(Console.ReadLine());
        s_bl!.Task.DesignateEngineer(taskId, engineerId);
    }


    //The method deletes a task given by the user(the user enters the id)
    private static void TaskDelete()
    {
        Console.WriteLine("Enter the task id to delete:");
        int _taskId = Convert.ToInt32(Console.ReadLine());
        s_bl!.Task.Delete(_taskId);
    }

    private static void TaskUpdateSchuledDate()
    {
        Console.WriteLine("enter task id:");
        int taskId = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("enter schedule date:");
        DateTime scheduleDate = ProjectImplementation.GetDateTimeFromUser();
        s_bl!.Task.Update(taskId, scheduleDate);
    }

    static void Main(string[] args)
    {
        Console.Write("Would you like to create Initial data? (Y/N)");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans == "Y")
            DalTest.Initialization.Do();
        MainMenu();
    }
    #endregion

    #region stage 1
    public static void FirstStage()
    {
        string answer = "n";
        Console.WriteLine("enter all the tasks that are necessery for the project");

        do
        {
            TaskCreate();
            Console.WriteLine("\n\n\ndo you want to add another task?");
            answer = Console.ReadLine();
        } while (answer == "y");
    }
    #endregion
}

