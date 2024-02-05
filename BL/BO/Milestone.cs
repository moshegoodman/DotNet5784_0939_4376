namespace BO;
public class Milestone
{
    public int Id { get; init; }
    string Description { get; set; }
    string Alias { get; set; }
    BO.Status status { get; set; }
    DateTime CreatedAtDate { get; init; }
    DateTime? ForecastDate { get; set; }
    DateTime? DeadlineDate { get; set; }
    DateTime? CompleteDate { get; set; }
    double? CompletionPercentage { get; set; }
    string? Remarks { get; set; }
    List<BO.TaskInList>? Dependencies { get; set; }
}
