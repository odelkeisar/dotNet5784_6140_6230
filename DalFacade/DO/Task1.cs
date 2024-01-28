namespace DO;
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
public record Task1
(
    int Id,
    string? Alias = null, //כינוי
    string? Description = null,//תיאור
    DateTime? CreatedAtDate = null,//תאריך יצירת המשימה
    DateTime? ScheduledDate = null,//תאריך מתוכנן להתחלה
    TimeSpan? RequiredEffortTime = null,//משך זמן המשימה
    DateTime? DeadlineDate = null,//תאריך סיום
    int ChefId = 0,
    DateTime? StartDate = null,//התחלה בפועל
    DateTime? CompleteDate = null,//סיום בפועל
    ChefExperience? Copmlexity = null, //רמת קושי
    string? Dellverables = null, //תוצר
    string? Remarks = null,//הערות
    bool ?isMileStone = false//אבני דרך
)
{
    public Task1() : this(0) { }
}

