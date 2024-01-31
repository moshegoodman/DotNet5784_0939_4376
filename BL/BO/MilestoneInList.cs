namespace BO;

public class MilestoneInList
{
    int Id { get; init; }
    DateTime CreatedAtDate { get; init; }
    string Description { get; set; }
    string Alias { get; set; }
    BO.Status status {  get; set; }
    double? CompletionPercentage { get; set; }
  
}
