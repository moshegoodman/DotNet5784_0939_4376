namespace BO;
/// <summary>
/// 
/// </summary>
/// 
/// <param name="Id">A unique number for each task</param>
/// <param name="Alias">An optional alias or alternate identifier for the task</param>
/// <param name="Description">A description providing details about the task</param>
/// <param name="CreatedAtDate">The date and time when the task was created</param>
/// <param name="Status">The date and time when the task was created</param>
/// <param name="Dependencies">A list of the the tasks that must be done before this one</param>

/// <param name="IsMilestone">A boolean indicating whether the task is a milestone</param>
/// <param name="Complexity">An enumeration representing the complexity level of the task (using the "DO.EngineerExperience" type)</param>
/// <param name="Deliverables">Any deliverables associated with the task</param>
/// <param name="Remarks">Additional remarks or comments related to the task</param>
/// <param name="RequiredEffortTime">The estimated time required to complete the task</param>
/// <param name="StartDate">The real start date for the task</param>
/// <param name="ScheduledDate">The scheduled date for the task</param>
/// <param name="ForecastDate">The expected date of copmleting the rtask</param>
/// <param name="DeadlineDate">The deadline by which the task should be completed</param>
/// <param name="CompleteDate">The actual completion date of the task</param>
/// <param name="Engineer">Engineer that is in charge of the task</param>
public class Task
{

    public int Id { get; init; }
    public string Alias { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAtDate { get; init; }
    public BO.Status Status { get; set; }
    public List<BO.TaskInList> Dependencies { get; set; }
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


    public override string ToString()
    {
        string a = $"ID:\t{Id}\nAlias:\t{Alias}\nDescription:\t{Description}\nCreatedAtDate:\t{CreatedAtDate}\nScheduledDate:\t{ScheduledDate}\n";
        a = a + $"Status\t{Status}\n";
        //foreach (var item in Dependencies)
        //{
        //    a = a + item;
        //}
        return a;
    }
}
