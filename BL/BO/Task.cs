namespace BO;
public class Task
{
    int Id { get; init; }
    string Alias { get; set; }
    string Description { get; set; }
    DateTime CreatedAtDate { get; init; }
    BO.Status Status { get; set; }
    List<BO.TaskInList> Dependencies { get; set; }
    BO.MilestoneInTask? Milestone { get; set; }
    BO.EngineerExperience Complexity { get; set; }
    string? Deliverables { get; set; }
    string? Remarks { get; set; }
    TimeSpan? RequiredEffortTime { get; set; }
    DateTime? StartDate { get; set; }
    DateTime? ScheduledDate { get; set; }
    DateTime? ForecastDate { get; set; }
    DateTime? DeadlineDate { get; set; }
    DateTime? CompleteDate { get; set; }
    BO.EngineerInTask? Engineer { get; set; }
}
