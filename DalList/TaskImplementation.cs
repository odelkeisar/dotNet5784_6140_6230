namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Implementing methods for the tasks data structure.
/// </summary>
internal class TaskImplementation : ITask1
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
            throw new DalDoesNotExistException($"Task with ID={id} does not exist");
        DataSource.Tasks.Remove(item1);
    }

    public Task1? Read(int id)
    {
        //Task1? item = DataSource.Tasks.Find(x => x.Id == id);

        //if (item == null) return null;
        //return item;

        return DataSource.Tasks.FirstOrDefault(x => (x.Id == id));
    }

    public Task1? Read(Func<Task1, bool> filter) // stage 2
    {
        return DataSource.Tasks.FirstOrDefault(filter);
    }

    //public List<Task1> ReadAll()
    //{
    //    return new List<Task1>(DataSource.Tasks);
    //}
    public IEnumerable<Task1> ReadAll(Func<Task1, bool>? filter = null)
    {
        if (filter == null)
            return DataSource.Tasks.Select(item => item).ToList();
        else
            return DataSource.Tasks.Where(filter).ToList();
    }

    public void Update(Task1 item)
    {
        Task1? item1 = Read(item.Id);

        if (item1 == null)
            throw new DalDoesNotExistException($"Task with ID={item.Id} does not exist");

        DataSource.Tasks.Remove(item1); 

        DataSource.Tasks.Add(item);
    }
}
