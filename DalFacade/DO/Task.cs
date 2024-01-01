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
public record Task
(
    int id,
    int engineerId,
    DateTime? StartDate,
    DateTime? ScheduledDate,
    DateTime? DeadlineDate,
    TimeSpan? RequiredEffortTime,
    DO.EngineerExperience? Copmlexity,
    string ? Dellverables = null,
    string? Remarks = null,
    string? Alias=null,
    string? Description = null,
    DateTime? CreatedAtDate=null,
    bool isMileStone=false
);

