namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.ComponentModel.Design;

/// <summary>
/// Implementation of methods for the data structure of engineers.
/// </summary>
public class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
       Engineer? item1 = Read(item.Id);
        
        if (item1 != null)
            throw new Exception("An object of type Engineer with such an ID already exists ");

       DataSource.Engineers.Add(item);
       return item.Id;
    }

    public void Delete(int id)
    {
        Engineer? item1 = Read(id);
        
        if (item1 == null)
            throw new Exception("An object of type T with such an ID does not exist");
        DataSource.Engineers.Remove(item1);
    }

    public Engineer? Read(int id)
    {
        Engineer? item = DataSource.Engineers.Find(x => x.Id == id);
       
        if (item == null) return null;
        return item;
    }

    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }

    public void Update(Engineer item)
    {
        Engineer? item1 = Read(item.Id);

        if (item1 == null)
            throw new Exception("An object of type Engineer with such an ID does not exist");

        DataSource.Engineers.Remove(item1);
        DataSource.Engineers.Add(item);
    }
}
