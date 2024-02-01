namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

/// <summary>
/// Implementing methods for the tasks data structure.
/// </summary>
internal class TaskImplementation : ITask1
{
    /// <summary>
    /// add a new member of type task1
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Task1 item)
    {
        int newId = DataSource.Config.NextTaskId;
        Task1 item1 = item with { Id = newId };
        DataSource.Tasks!.Add(item1);    //add item to the list
        return newId;   
    }

    /// <summary>
    /// delete member from the list according the id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        Task1? item1 = Read(id);
   
        if (item1 == null)
            throw new DalDoesNotExistException($"Task with ID={id} does not exist");
        DataSource.Tasks!.Remove(item1);
    }

    /// <summary>
    /// The function checks which value to return by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task1? Read(int id)
    {
        return DataSource.Tasks!.FirstOrDefault(x => (x.Id == id)); //Returns the first entry in the list with this ID
    }

    /// <summary>
    /// The function checks which value to return according to the condition in the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Task1? Read(Func<Task1, bool> filter) 
    {
        return DataSource.Tasks!.FirstOrDefault(filter); //Returns the first value in the list equal to the filter
    }

    /// <summary>
    /// The function returns all elements in the list or it returns only those that meet the condition in the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Task1> ?ReadAll(Func<Task1, bool>? filter = null)
    {
        if (filter == null)
            return DataSource.Tasks!.Select(item => item).ToList(); //retun the list
        else
            return DataSource.Tasks!.Where(filter).ToList();
    }

    /// <summary>
    /// The method edits an element from the list according to the user's request
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Task1 item)
    {
        Task1? item1 = Read(item.Id);

        if (item1 == null)
            throw new DalDoesNotExistException($"Task with ID={item.Id} does not exist");

        DataSource.Tasks!.Remove(item1); //remove item1 from the list

        DataSource.Tasks.Add(item); //add item to the list
    }

    public void DeleteAll() { DataSource.Tasks!.Clear(); }
}
