using BlApi;
namespace BlTest;


internal class Program
{

    static readonly IBl s_bl = Factory.Get();

    readonly string s_engineers_xml = "engineers";
    static string s_data_config_xml = "data-config";
    public static bool ValidScheduleDate(int taskId, DateTime _scheduleDate)
    {
        bool flag = true;
        BO.Task task = s_bl.Task.Read(taskId);
        (task.Dependencies).ForEach(task =>
        {
            if (_scheduleDate < s_bl.Task.Read(task.Id).ScheduledDate)
            {
                flag = false;
            }
            if (s_bl.Task.Read(task.Id).ScheduledDate == null)
                throw new BO.DependentScheduleNotInitialized($"Dependent Schedule Not Initialized");

        });
        return flag;
    }
    public static DateTime GetDateTimeFromUser()
    {
        string userInput = Console.ReadLine();
        DateTime projectScheduledDate;
        // Parse the user input into a TimeSpan

        while (!(DateTime.TryParse(userInput, out projectScheduledDate)))
        {
            Console.WriteLine("Invalid DateTime format. Please try again.");
            userInput = Console.ReadLine();
        }
        return projectScheduledDate;
    }
    public static TimeSpan GetTimeSpanFromUser()
    {
        string userInput = Console.ReadLine();
        TimeSpan projectScheduledDate;
        // Parse the user input into a TimeSpan

        while (!(TimeSpan.TryParse(userInput, out projectScheduledDate)))
        {
            Console.WriteLine("Invalid DateTime format. Please try again.");
            userInput = Console.ReadLine();
        }
        return projectScheduledDate;
    }


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
        TimeSpan _requiredEffortTime = GetTimeSpanFromUser();
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
        DateTime? _createdAtDate = GetDateTimeFromUser();

        Console.WriteLine("Enter the new complexity of the task 1-5:");
        BO.EngineerExperience _taskComplexity = (BO.EngineerExperience)Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Enter the deliverables of the task:");
        string _deliverables = Console.ReadLine()!;

        Console.WriteLine("Enter the remark of the task:");
        string _remark = Console.ReadLine()!;

        Console.WriteLine("Enter if the task is milestone:  Y/N");
        bool isMilestone = (Console.ReadLine()! == "Y");

        Console.WriteLine("Enter how many time it will take, by following these steps!");
        TimeSpan? requiredEffortTime = GetTimeSpanFromUser();

        Console.WriteLine("Enter the end-date of the task by following these steps!");
        DateTime? startDate = GetDateTimeFromUser();

        Console.WriteLine("Enter the start-date of the task by following these steps!");
        DateTime? scheduledDate = GetDateTimeFromUser();

        Console.WriteLine("Enter the dead-line date of the task by following these steps!");
        DateTime? deadlineDate = GetDateTimeFromUser();

        Console.WriteLine("Enter the real end-date of the task by following these steps!");
        DateTime? completeDate = GetDateTimeFromUser();

        Console.WriteLine("Enter the engineer's id of the task:");
        int engineerId = Convert.ToInt32(Console.ReadLine());

        BO.Status status = BO.Status.Unscheduled; //i dont care what the value is
        List<BO.TaskInList> dependencies = new List<BO.TaskInList>();
        DateTime forecastDate = new DateTime();
        BO.EngineerInTask? engineer = new();
        BO.Task newTask = new BO.Task()
        {
            Id = _id,
            Alias = _alias,
            Description = _description,
            CreatedAtDate = (System.DateTime)_createdAtDate,
            Status = BO.Status.Unscheduled,
            Dependencies = dependencies,
            Milestone = null,
            Complexity = _taskComplexity,
            Deliverables = _deliverables,
            Remarks = _remark,
            RequiredEffortTime = requiredEffortTime,
            StartDate = startDate,
            ScheduledDate = scheduledDate,
            ForecastDate = forecastDate,
            DeadlineDate = deadlineDate,
            CompleteDate = completeDate,
            Engineer = engineer
        };
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
        DateTime scheduleDate = GetDateTimeFromUser();
        s_bl!.Task.Update(taskId, scheduleDate);
    }


    #endregion

    #region engineer
    private static void EngineerMenu()
    {
        int a;
        do
        {
            Console.WriteLine("Enter 1 to exit the engineers menu");
            Console.WriteLine("Enter 2 to add an new engineer ");
            Console.WriteLine("Enter 3 to read an engineer ");
            Console.WriteLine("Enter 4 to read all engineers ");
            Console.WriteLine("Enter 5 to update an engineer ");
            Console.WriteLine("Enter 6 to delete an engineer");
            a = Convert.ToInt32(Console.ReadLine())!;
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
        while (a != 0);
    }


    private static void EngineerCreate()
    {
        Console.WriteLine("Enter engineers id:");
        int _engineer_id = Convert.ToInt32(Console.ReadLine())!;

        Console.WriteLine("Enter engineers email:");
        string email = Console.ReadLine()!;

        Console.WriteLine("Enter engineers price per hour:");
        double cost = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine("Enter engineers name:");
        string name = Console.ReadLine()!;

        Console.WriteLine("Enter engineers level:");
        BO.EngineerExperience level = (BO.EngineerExperience)Convert.ToInt32(Console.ReadLine());

        BO.Engineer newEngineer = new BO.Engineer()
        {
            Id = _engineer_id,
            Name = name,
            Cost = cost,
            Email = email,
            Level = level,
            Task = null
        };
        Console.WriteLine("new enginerr id:");
        Console.WriteLine(s_bl!.Engineer.Create(newEngineer));
    }

    private static void EngineerRead()
    {
        Console.WriteLine("Enter the dependancy id:");
        int _engineer = Convert.ToInt32(Console.ReadLine())!;

        Console.WriteLine(s_bl!.Engineer.Read(_engineer));
    }

    private static void EngineerReadAll()
    {
        IEnumerable<BO.Engineer> newList = s_bl.Engineer.ReadAll()!;
        foreach (BO.Engineer engineer in newList) { Console.WriteLine(engineer); }
    }


    private static void EngineerUpdate()
    {
        Console.WriteLine("Enter engineers id:");
        int _engineerId = Convert.ToInt32(Console.ReadLine())!;


        if (s_bl!.Engineer.Read(_engineerId) == null)
            return;

        Console.WriteLine(s_bl!.Engineer.Read(_engineerId));


        Console.WriteLine("Enter engineers email:");
        string email = Console.ReadLine()!;

        Console.WriteLine("Enter engineers price per hour:");
        double cost = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine("Enter engineers name:");
        string name = Console.ReadLine()!;

        Console.WriteLine("Enter engineers level:");
        BO.EngineerExperience level = (BO.EngineerExperience)Convert.ToInt32(Console.ReadLine());



        //BO.Engineer newEngineer = new(_engineerId, Email, Cost!.Value, Name, Level, null);
        BO.Engineer newEngineer = new BO.Engineer()
        {
            Id = _engineerId,
            Name = name,
            Cost = cost,
            Email = email,
            Level = level,
            Task = null
        };

        s_bl!.Engineer.Update(newEngineer);
    }

    private static void EngineerDelete()
    {
        Console.WriteLine("Enter the engineer id to delete:");
        int _engineer = Convert.ToInt32(Console.ReadLine())!;
        s_bl!.Engineer.Delete(_engineer);
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


    #region stage 2

    public static void SecondStage()
    {
        Console.WriteLine("enter expected start of the project");
        DateTime projectScheduledDate = GetDateTimeFromUser();

        IEnumerable<BO.TaskInList> _allBoTasks = s_bl.Task.ReadAll();
        _allBoTasks.ToList().ForEach(task =>
        {
            Console.WriteLine($"Schedule a date for {task.Alias}");
            s_bl.Task.Update(task.Id, GetDateTimeFromUser());
        });
    }



    #endregion

    #region stage 3

    public static void ThirdStage()
    {
        string answer = "n";
        Console.WriteLine("do you want to designate a task to an engineer?");
        answer = Console.ReadLine();
        do
        {
            TaskDesignateEngineer();
            Console.WriteLine("do you want to designate another task to an engineer?");
            answer = Console.ReadLine();
        } while (answer == "y");
    }



    #endregion 
    static void Main(string[] args)
    {
        Console.Write("Would you like to create Initial data? (Y/N)");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans == "Y")
            DalTest.Initialization.Do();
        MainMenu();
    }
}

