

namespace BO;
public class Task
{
    private bool isMilestone;
    private DO.EngineerExperience taskComplexity;
    private string remark;
    private int engineerId;
    private object value;



    public int Id { get; init; }
    public string Alias { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAtDate { get; init; }
    public BO.Status Status { get; set; }
    public List<BO.TaskInList> Dependencies { get; set; }
    public BO.MilestoneInTask? Milestone { get; set; }
    public BO.EngineerExperience Complexity { get; set; }
    public string? Deliverables { get; set; }
    public string? Remarks { get; set; }
    public TimeSpan? RequiredEffortTime { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public DateTime? ForecastDate { get; set; }
    public DateTime? DeadlineDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public BO.EngineerInTask? Engineer { get; set; }

}
