using DO;
using System.Xml.Linq;

namespace DalApi;
/// <summary>
/// call to Icrud with type of task
/// </summary>
public interface ITask1 : ICrud<Task1> 
{
    public void UpdateStartProject(DateTime startProject);
    public void UpdateEndtProject( DateTime endProject);
    public DateTime? ReadStartProject();
    public DateTime? ReadEndProject();
}


