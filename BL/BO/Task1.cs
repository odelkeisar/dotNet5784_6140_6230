namespace BO;

/// <summary>
/// The task that the engineer performs, opening date, last date to finish it, level of difficulty, etc.
/// </summary>
/// <param name="id">task number</param> 
/// <param name="engineerId">An engineer working on the task</param>
/// <param name="Alias">The name of the task</param>
/// <param name="Description">Mission description</param>
/// <param name="CreatedAtDate">The task opening date</param>
/// <param name="RequiredEffortTime"> the level of effort</param>
/// <param name="isMileStone">Milestones</param>
/// <param name="Copmlexity">the complexity of the task</param>
/// <param name="StartDate">opening date</param>
/// <param name="ScheduledDate">The date the task was scheduled</param>
/// <param name="DeadlineDate">Deadline for the assignment</param>
/// <param name="Dellverables"></param>
/// <param name="Remarks">Notes for the assignment</param>
public class Task1
{
    public int Id { get; init; }
    public string? Alias { get; set; } //כינוי
    public string? Description { get; set; }//תיאור
    public Status status { get; set; }
    public List<TaskInList> ?dependeencies { get; set;}
    public DateTime? CreatedAtDate { get; set; } //תאריך יצירת המשימה
    public DateTime? ScheduledDate { get; set; } //תאריך מתוכנן להתחלה
    public DateTime? StartDate { get; set; }//התחלה בפועל
    public DateTime? ForecastDate 
    {
        get { return ScheduledDate < StartDate ? StartDate + RequiredEffortTime : ScheduledDate + RequiredEffortTime; }
    }
    public DateTime? DeadlineDate { get; set; }//תאריך סיום
    public DateTime? CompleteDate { get; set; } //סיום בפועל
    public TimeSpan? RequiredEffortTime { get; set; } //משך זמן המשימה
    public string? Dellverables { get; set; } //תוצר
    public string? Remarks { get; set; }//הערות
    public ChefInTask? chef { get; set; } 
    public  ChefExperience? Copmlexity { get; set; } //רמת קושי
    public override string ToString() => this.ToString();
}
