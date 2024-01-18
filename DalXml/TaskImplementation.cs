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

    /// <summary>
    /// //add dependeency to a task
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Task1 item)
    {
        var listTask = XMLTools.LoadListFromXMLSerializer<Task1>(s_tasks_xml);
        int newId = Config.NextTask1Id;
        Task1 item1 = item with { Id = newId };
        listTask!.Add(item1);    //add item to the list
        XMLTools.SaveListToXMLSerializer(listTask, s_tasks_xml);
        return newId;
    }

    /// <summary>
    /// delete member from the list according the id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        var listTask = XMLTools.LoadListFromXMLSerializer<Task1>(s_tasks_xml);
        Task1? item1 = Read(id);

        if (item1 == null)
            throw new DalDoesNotExistException($"Task with ID={id} does not exist");
        listTask!.Remove(item1);

        XMLTools.SaveListToXMLSerializer(listTask, s_tasks_xml);
    }

    /// <summary>
    /// The function checks which value to return by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task1? Read(int id)
    {
        var listTask = XMLTools.LoadListFromXMLSerializer<Task1>(s_tasks_xml);
        return listTask!.FirstOrDefault(x => (x.Id == id)); //Returns the first entry in the list with this ID

    }

    /// <summary>
    /// The function checks which value to return according to the condition in the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Task1? Read(Func<Task1, bool> filter)
    {
        var listTask = XMLTools.LoadListFromXMLSerializer<Task1>(s_tasks_xml);
        return listTask!.FirstOrDefault(filter); //Returns the first value in the list equal to the filter
    }

    /// <summary>
    /// The function returns all elements in the list or it returns only those that meet the condition in the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Task1?>? ReadAll(Func<Task1, bool>? filter = null)
    {
        var listTask = XMLTools.LoadListFromXMLSerializer<Task1>(s_tasks_xml);
        if (filter == null)
            return listTask!.Select(item => item).ToList(); //retun the list
        else
            return listTask!.Where(filter).ToList();
    }

    /// <summary>
    /// The method edits an element from the list according to the user's request
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
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
