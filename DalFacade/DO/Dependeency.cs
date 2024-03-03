namespace DO;
/// <summary>
/// Indicates the dependency between tasks, i.e. which task can only be executed after the completion of a previous task.
/// </summary>
/// <param name="Id">Unique ID number (automatic runner number)</param>
/// <param name="DependentTask">ID number of pending task</param>
/// <param name="DependsOnTask">Previous assignment ID number</param>
public record Dependeency
(
    int Id,
    int DependentTask, //משימה תלויה
    int DependsOnTask //תלויה במשימה
 )
{
    public Dependeency() : this(0,0,0) { }
}


