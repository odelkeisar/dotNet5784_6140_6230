using DalApi;
using DO;
using System.Data.Common;
using System.Xml.Linq;

namespace Dal;
internal class TaskImplementation : ITask1
{
    readonly string s_tasks_xml = "tasks";
    string taskPath = @"tasks.xml";
    public TaskImplementation()
    {
        try
        {
            XMLTools.LoadListFromXMLSerializer<Task1>("tasks");
        }
        
        catch(DalXMLFileLoadCreateException ex)
        {
            Console.WriteLine("File upload problem");
        }
    }

    public int Create(Task1 item)
    {
        var listTask = XMLTools.LoadListFromXMLSerializer<Task1>("tasks");
        int newId = Config.NextDependeencyId;
        Task1 item1 = item with { Id = newId };
        listTask!.Add(item1);    //add item to the list
        XMLTools.SaveListToXMLSerializer(listTask, "tasks");
        return newId;
    }

    public void Delete(int id)
    {
        var listTask = XMLTools.LoadListFromXMLSerializer<Task1>("tasks");
        Task1? item1 = Read(id);

        if (item1 == null)
            throw new DalDoesNotExistException($"Task with ID={id} does not exist");
        listTask!.Remove(item1);

        XMLTools.SaveListToXMLSerializer(listTask, "tasks");
    }

    public Task1? Read(int id)
    {
        var listTask = XMLTools.LoadListFromXMLSerializer<Task1>("tasks");
        return listTask!.FirstOrDefault(x => (x.Id == id)); //Returns the first entry in the list with this ID

    }

    public Task1? Read(Func<Task1, bool> filter)
    {
        var listTask = XMLTools.LoadListFromXMLSerializer<Task1>("tasks");
        return listTask!.FirstOrDefault(filter); //Returns the first value in the list equal to the filter
    }

    public IEnumerable<Task1?> ReadAll(Func<Task1, bool>? filter = null)
    {
        var listTask = XMLTools.LoadListFromXMLSerializer<Task1>("tasks");
        if (filter == null)
            return listTask!.Select(item => item).ToList(); //retun the list
        else
            return listTask!.Where(filter).ToList();
    }

    public void Update(Task1 item)
    {
        var listTask = XMLTools.LoadListFromXMLSerializer<Task1>("tasks");

        Task1? item1 = Read(item.Id);

        if (item1 == null)
            throw new DalDoesNotExistException($"Task with ID={item.Id} does not exist");

        listTask!.Remove(item1); //remove item1 from the list

        listTask.Add(item); //add item to the list

        XMLTools.SaveListToXMLSerializer(listTask, "tasks");
    }
}
