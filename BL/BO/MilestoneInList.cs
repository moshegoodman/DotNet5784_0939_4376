namespace BO;

public class MilestoneInList
{
    public int Id { get; init; }
    public DateTime CreatedAtDate { get; init; }
    public string Description { get; set; }
    public string Alias { get; set; }
    public BO.Status status {  get; set; }
    public double? CompletionPercentage { get; set; }
  
}
