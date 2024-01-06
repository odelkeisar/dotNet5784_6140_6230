namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

/// <summary>
/// Implementing methods for the tasks data structure.
/// </summary>
public class TaskImplementation : ITask1
{ 
    public int Create(Task1 item)
    {
        int newId = DataSource.Config.NextTaskId;
        Task1 item1 = item with { Id = newId };
        DataSource.Tasks.Add(item1);    
        return newId;   
    }

    public void Delete(int id)
    {
        Task1? item1 = Read(id);
   
        if (item1 == null)
            throw new Exception("An object of type Task with such an ID does not exist");
        DataSource.Tasks.Remove(item1);
    }

    public Task1? Read(int id)
    {
        Task1? item = DataSource.Tasks.Find(x => x.Id == id);

        if (item == null) return null;
        return item;

        throw new NotImplementedException();
    }

    public List<Task1> ReadAll()
    {
        return new List<Task1>(DataSource.Tasks);
    }

    public void Update(Task1 item)
    {
        Task1? item1 = Read(item.Id);

        if (item1 == null)
            throw new Exception("An object of type Task with such an ID does not exist");

        DataSource.Tasks.Remove(item1); 

        DataSource.Tasks.Add(item);
    }
}
