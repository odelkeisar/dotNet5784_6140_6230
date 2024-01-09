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
            throw new Exception("An object of type Chef with such an ID already exists ");

       DataSource.Chefs.Add(item); //add item to the list
        return item.Id;
    }

    public void Delete(int id)
    {
        Chef? item1 = Read(id);
        
        if (item1 == null)
            throw new Exception("An object of type T with such an ID does not exist");
        DataSource.Chefs.Remove(item1); //remove item1 from the list
    }

    public Chef? Read(int id)
    {
        Chef? item = DataSource.Chefs.Find(x => x.Id == id); //Finds the entry in the list that is his ID 

        if (item == null) return null;
        return item;
    }

    public List<Chef> ReadAll()
    {
        return new List<Chef>(DataSource.Chefs);
    }

    public void Update(Chef item)
    {
        Chef? item1 = Read(item.Id);

        if (item1 == null)
            throw new Exception("An object of type Engineer with such an ID does not exist");

        DataSource.Chefs.Remove(item1); //remove item1 from the list
        DataSource.Chefs.Add(item); //add item to the list
    }
}
