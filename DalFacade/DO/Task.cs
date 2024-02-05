namespace DO;
/// <summary>
/// 
/// </summary>
/// <param name="Id">A unique number for each task</param>
/// <param name="Alias">An optional alias or alternate identifier for the task</param>
/// <param name="Description">A description providing details about the task</param>
/// <param name="CreatedAtDate">The date and time when the task was created</param>
/// <param name="RequiredEffortTime">The estimated time required to complete the task</param>
/// <param name="IsMilestone">A boolean indicating whether the task is a milestone</param>
/// <param name="Complexity">An enumeration representing the complexity level of the task (using the "DO.EngineerExperience" type)</param>
/// <param name="StartDate">The planned start date for the task</param>
/// <param name="ScheduledDate">The scheduled date for the task</param>
/// <param name="DeadlineDate">The deadline by which the task should be completed</param>
/// <param name="CompleteDate">The actual completion date of the task</param>
/// <param name="Deliverables">Any deliverables associated with the task</param>
/// <param name="Remarks">Additional remarks or comments related to the task</param>
/// <param name="EngineerId">The identifier of the engineer assigned to the task</param>
public record Task
(
    int Id,
    string Alias,
    string Description,
    DateTime CreatedAtDate,
    DO.EngineerExperience Complexity = EngineerExperience.Beginner,
    string? Deliverables = null,
    string? Remarks = null,
    bool IsMilestone = false,
    TimeSpan? RequiredEffortTime = null,
    DateTime? StartDate = null,
    DateTime? ScheduledDate = null,
    DateTime? DeadlineDate = null,
    DateTime? CompleteDate = null,
    int? EngineerId = null
)
{
    Task() : this(0, "", "", DateTime.Now) { }
}

