namespace BlImplementation;
using System;

public class ProjectImplementation
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

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

