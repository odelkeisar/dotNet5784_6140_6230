namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

/// <summary>
/// Implementation of methods for the data structure of Dependeency
/// </summary>
internal class DependeencyImplementation : IDependeency
{
    /// <summary>
    /// add a new member of type depeneency
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Dependeency item)
    {
        int newId = DataSource.Config.NextDependeencyId;
        Dependeency item1 = item with { Id = newId };
        DataSource.Dependeencies!.Add(item1);
        return newId;
    }

    /// <summary>
    /// delete member from the list according the id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        Dependeency? item = Read(id);

        if (item == null)
            throw new DalDoesNotExistException($"Dependeency with ID={id} does not exist");
        DataSource.Dependeencies!.Remove(item); //remove item from the list
    }

    /// <summary>
    /// The function checks which value to return by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Dependeency? Read(int id)
    {

        return DataSource.Dependeencies!.FirstOrDefault(x => (x.Id == id)); //Returns the first entry in the list with this ID
    }

    /// <summary>
    /// The function checks which value to return according to the condition in the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Dependeency? Read(Func<Dependeency, bool> filter) 
    {
        return DataSource.Dependeencies!.FirstOrDefault(filter); //Returns the first value in the list equal to the filter
    }

    /// <summary>
    /// The function returns all elements in the list or it returns only those that meet the condition in the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Dependeency> ReadAll(Func<Dependeency, bool>? filter = null)
    {
        if (filter == null)
            return DataSource.Dependeencies!.Select(item => item).ToList(); //retun the list
        else
            return DataSource.Dependeencies!.Where(filter).ToList();
    }

    /// <summary>
    /// The method edits an element from the list according to the user's request
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Dependeency item)
    {
        Dependeency? item1 = Read(item.Id);

        if (item1 == null)
            throw new DalDoesNotExistException($"Dependeency with ID={item.Id} does not exist");

        DataSource.Dependeencies!.Remove(item1); //remove item1 from the list
        DataSource.Dependeencies.Add(item); //add item to the list
    }
    public void DeleteAll() { DataSource.Dependeencies!.Clear(); }
}
