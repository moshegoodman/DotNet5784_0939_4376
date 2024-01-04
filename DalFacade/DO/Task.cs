
namespace DO;
public record Task
(
    int id,
    string Alius,
    string Description,
    datetime CreatedAtDate,
    TimeSpan RequiredEffortTime,
    bool IsMilestone,
    DO.enum Complexity,
    DateTime StartDate,
    datetime ScheduledDate,
    datetime DeadlineDate,
    datetime CompleteDate,
    string Deliverables,
    string Remarks,
    int EngineerId
{

}

