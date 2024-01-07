namespace DO;
public record Task
(
    int Id,
    string Alias,
    string Description,
    DateTime CreatedAtDate,
    TimeSpan RequiredEffortTime,
    bool IsMilestone,
    DO.EngineerExperience Complexity,
    DateTime StartDate,
    DateTime ScheduledDate,
    DateTime DeadlineDate,
    DateTime CompleteDate,
    string Deliverables,
    string Remarks,
    int EngineerId
)
{

}

