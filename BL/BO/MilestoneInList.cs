namespace BO;
/// <summary>
/// milestones-in-list helper entity - for the milestones list screen
/// </summary>
public class MilestoneInList
{
    public int Id { get; init; }
    public string? Description { get; set; }
    public string? Alias { get; set; }
    public Status? Status { get; set; }
    public double? CompletionPercentage { get; set; }
}
