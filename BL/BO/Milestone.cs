namespace BO;
/// <summary>
/// Primary logical entity of a milestone
/// </summary>
public class Milestone
{
    public int Id { get; init; }
    public string? Alias { get; set; }
    public string? Description { get; set; }
    public DateTime? CreatedDate { get; set;}
    public Status? Status { get; set; }
    public DateTime? ForecastDate { get; set; }
    public DateTime? DeadlineDate { get;set;}
    public DateTime? CompleteDate { get; set; }
    public double? CompletionPercentage { get; set; }
    public string? remarks { get; set; }
    public List<BO.TaskInList>? Dependencies { get; set; }


}
