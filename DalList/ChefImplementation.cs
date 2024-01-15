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
    public int Create(Chef item)
    {
       Chef? item1 = Read(item.Id);
        
        if (item1 != null)
            throw new DalAlreadyExistsException($"An object of type Chef with {item.Id} already exists ");

       DataSource.Chefs!.Add(item); //add item to the list
        return item.Id;
    }

    public void Delete(int id)
    {
        Chef? item1 = Read(id);
        
        if (item1 == null)
            throw new DalDoesNotExistException($"Dependeency with ID={id} does not exist");
        DataSource.Chefs!.Remove(item1); //remove item1 from the list
    }

    public Chef? Read(Func<Chef, bool> filter)
    {
        return DataSource.Chefs!.FirstOrDefault(filter); //Returns the first value in the list equal to the filter
    }
    public Chef? Read(int id)
    {
        Chef? item = DataSource.Chefs!.Find(x => x.Id == id); //Finds the entry in the list that is his ID 

        if (item == null) return null;
        return item;
    }

    public IEnumerable<Chef> ReadAll(Func<Chef, bool>? filter = null)
    {
        if (filter == null)
            return DataSource.Chefs!.Select(item => item).ToList(); //retun the list
        else
            return DataSource.Chefs!.Where(filter).ToList();
    }

    public void Update(Chef item)
    {
        Chef? item1 = Read(item.Id);

        if (item1 == null)
            throw new DalDoesNotExistException($"Dependeency with ID={item.Id} does not exist");

        DataSource.Chefs!.Remove(item1); //remove item1 from the list
        DataSource.Chefs.Add(item); //add item to the list
    }
}
