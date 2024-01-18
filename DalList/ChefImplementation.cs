namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.ComponentModel.Design;

/// <summary>
/// Implementation of methods for the data structure of chef.
/// </summary>
public class ChefImplementation : IChef
{
    /// <summary>
    /// add a new member of type chef
    /// </summary>
    /// <param name="item"> item is the member that add to the list </param>
    /// <returns></returns>
    /// <exception cref="DalAlreadyExistsException"></exception>
    public int Create(Chef item)
    {
       Chef? item1 = Read(item.Id);
        
        if (item1 != null)
            throw new DalAlreadyExistsException($"An object of type Chef with {item.Id} already exists ");

       DataSource.Chefs!.Add(item); //add item to the list
        return item.Id;
    }

    /// <summary>
    /// delete member from the list according the id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        Chef? item1 = Read(id);
        
        if (item1 == null)
            throw new DalDoesNotExistException($"Dependeency with ID={id} does not exist");
        DataSource.Chefs!.Remove(item1); //remove item1 from the list
    }

    /// <summary>
    /// The function checks which value to return according to the condition in the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Chef? Read(Func<Chef, bool> filter)
    {
        return DataSource.Chefs!.FirstOrDefault(filter); //Returns the first value in the list equal to the filter
    }

    /// <summary>
    /// The function checks which value to return by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Chef? Read(int id)
    {
        Chef? item = DataSource.Chefs!.Find(x => x.Id == id); //Finds the entry in the list that is his ID 

        if (item == null) return null;
        return item;
    }

    /// <summary>
    /// The function returns all elements in the list or it returns only those that meet the condition in the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Chef> ReadAll(Func<Chef, bool>? filter = null)
    {
        if (filter == null)
            return DataSource.Chefs!.Select(item => item).ToList(); //retun the list
        else
            return DataSource.Chefs!.Where(filter).ToList();
    }

    /// <summary>
    /// The method edits an element from the list according to the user's request
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Chef item)
    {
        Chef? item1 = Read(item.Id);

        if (item1 == null)
            throw new DalDoesNotExistException($"Dependeency with ID={item.Id} does not exist");

        DataSource.Chefs!.Remove(item1); //remove item1 from the list
        DataSource.Chefs.Add(item); //add item to the list
    }
    public void DeleteAll() { DataSource.Chefs.Clear(); }
}
