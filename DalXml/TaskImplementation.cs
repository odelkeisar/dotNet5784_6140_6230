using DalApi;
using DO;
using System.Data.Common;
using System.Xml.Linq;

namespace Dal;
/// <summary>
/// Implementation of crud functions of task
/// </summary>
internal class TaskImplementation : ITask1
{
    readonly string s_tasks_xml = "tasks";
    string taskPath = @"tasks.xml";
    public TaskImplementation()
    {
            XMLTools.LoadListFromXMLSerializer<Task1>(s_tasks_xml);
    }

    public int Create(Task1 item)
    {
        var listTask = XMLTools.LoadListFromXMLSerializer<Task1>(s_tasks_xml);
        int newId = Config.NextTask1Id;
        Task1 item1 = item with { Id = newId };
        listTask!.Add(item1);    //add item to the list
        XMLTools.SaveListToXMLSerializer(listTask, s_tasks_xml);
        return newId;
    }

    public void Delete(int id)
    {
        var listTask = XMLTools.LoadListFromXMLSerializer<Task1>(s_tasks_xml);
        Task1? item1 = Read(id);

        if (item1 == null)
            throw new DalDoesNotExistException($"Task with ID={id} does not exist");
        listTask!.Remove(item1);

        XMLTools.SaveListToXMLSerializer(listTask, s_tasks_xml);
    }

    public Task1? Read(int id)
    {
        var listTask = XMLTools.LoadListFromXMLSerializer<Task1>(s_tasks_xml);
        return listTask!.FirstOrDefault(x => (x.Id == id)); //Returns the first entry in the list with this ID

    }

    public Task1? Read(Func<Task1, bool> filter)
    {
        var listTask = XMLTools.LoadListFromXMLSerializer<Task1>(s_tasks_xml);
        return listTask!.FirstOrDefault(filter); //Returns the first value in the list equal to the filter
    }

    public IEnumerable<Task1?>? ReadAll(Func<Task1, bool>? filter = null)
    {
        var listTask = XMLTools.LoadListFromXMLSerializer<Task1>(s_tasks_xml);
        if (filter == null)
            return listTask!.Select(item => item).ToList(); //retun the list
        else
            return listTask!.Where(filter).ToList();
    }

    public void Update(Task1 item)
    {
        var listTask = XMLTools.LoadListFromXMLSerializer<Task1>(s_tasks_xml);

        Task1? item1 = Read(item.Id);

        if (item1 == null)
            throw new DalDoesNotExistException($"Task with ID={item.Id} does not exist");

        listTask!.Remove(item1); //remove item1 from the list

        listTask.Add(item); //add item to the list

        XMLTools.SaveListToXMLSerializer(listTask, s_tasks_xml);
    }
    public void DeleteAll() 
    {
        var listTask = XMLTools.LoadListFromXMLSerializer<Task1>(s_tasks_xml);
        listTask.Clear();   
        XMLTools.SaveListToXMLSerializer(listTask, s_tasks_xml);
    }
}
