namespace BlImplementation;
using System;

internal class ProjectImplementation
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    private static DateTime GetDateTimeFromUser()
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
    private static TimeSpan GetTimeSpanFromUser()
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

    private static bool ValidScheduleDate(int taskId, DateTime _scheduleDate)
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
    public static void FirstStage()
    {
        string? _remarks = null;
        string answer = "n";

        Console.WriteLine("enter all the tasks that are necessery for the project");

        do
        {

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


            Console.WriteLine("\n\n\ndo you want to add another task?");
            answer = Console.ReadLine();
        } while (answer == "y");
    }
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

}

