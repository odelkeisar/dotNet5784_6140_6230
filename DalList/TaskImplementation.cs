namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int newId = DataSource.Config.NextTaskId;
        Task item1=item with { Id = newId};
        DataSource.Tasks.Add(item1);    
        return newId;   
    }

    public void Delete(int id)
    {
        Task? item1 = Read(id);
   
        if (item1 == null)
            throw new Exception("An object of type Task with such an ID does not exist");
        DataSource.Tasks.Remove(item1);
    }

    public Task? Read(int id)
    {
        Task? item = DataSource.Tasks.Find(x => x.Id == id);

        if (item == null) return null;
        return item;

        throw new NotImplementedException();
    }

    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }

    public void Update(Task item)
    {
        Task? item1 = Read(item.Id);

        if (item1 == null)
            throw new Exception("An object of type Task with such an ID does not exist");

        DataSource.Tasks.Remove(item1); 

        DataSource.Tasks.Add(item);
    }
}
