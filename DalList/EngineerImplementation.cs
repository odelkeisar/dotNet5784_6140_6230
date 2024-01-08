namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

/// <summary>
/// Implementation of methods for the data structure of engineers.
/// </summary>
internal class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
       Engineer? item1 = Read(item.Id);
        
        if (item1 != null)
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");

       DataSource.Engineers.Add(item);
       return item.Id;
    }

    public void Delete(int id)
    {
        Engineer? item1 = Read(id);
        
        if (item1 == null)
            throw new DalDoesNotExistException($"Engineer with ID={id} does not exist");
        DataSource.Engineers.Remove(item1);
    }

    public Engineer? Read(int id)
    {
        Engineer? item = DataSource.Engineers.Find(x => x.Id == id);

        if (item == null) return null;
        return item;

        //return DataSource.Engineers.FirstOrDefault(x => (x.Id == id));
    }

    public Engineer? Read(Func<Engineer, bool> filter) 
    {
        return DataSource.Engineers.FirstOrDefault(filter);
    }

    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null)
    {
        if (filter == null)
            return DataSource.Engineers.Select(item => item).ToList();
        else
            return DataSource.Engineers.Where(filter).ToList();
    }

    public void Update(Engineer item)
    {
        Engineer? item1 = Read(item.Id);

        if (item1 == null)
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does not exist");

        DataSource.Engineers.Remove(item1);
        DataSource.Engineers.Add(item);
    }
}
