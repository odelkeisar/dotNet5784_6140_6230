using DO;

namespace DalApi;
/// <summary>
/// call to Icrud with type of task
/// </summary>
public interface ITask1 : ICrud<Task1> 
{
    public void UpdateStarEndtProject(DateTime startProject, DateTime endProject);
    public DateTime? ReadStartProject();
    public DateTime? ReadEndProject();
}


