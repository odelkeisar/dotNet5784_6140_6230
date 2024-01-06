namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

/// <summary>
/// Implementation of methods for the data structure of Dependeency
/// </summary>
public class DependeencyImplementation : IDependeency
{
    public int Create(Dependeency item)
    {
        int newId = DataSource.Config.NextDependeencyId;
        Dependeency item1 = item with { Id = newId };
        DataSource.Dependeencies.Add(item1);
        return newId;
    }

    public void Delete(int id)
    {
        Dependeency? item1 = Read(id);
        if (item1 == null)
            throw new Exception("An object of type Dependeency with such an ID does not exist");
        DataSource.Dependeencies.Remove(item1);
    }

    public Dependeency? Read(int id)
    {
       Dependeency? item= DataSource.Dependeencies.Find(x=> x.Id == id);

        if (item == null) return null;
        return item;
    }

    public List<Dependeency> ReadAll()
    {
        return new List<Dependeency>(DataSource.Dependeencies);
    }

    public void Update(Dependeency item)
    {
        Dependeency? item1 = Read(item.Id);

        if(item1 == null) 
        throw new Exception("An object of type Dependeency with such an ID does not exist");

        DataSource.Dependeencies.Remove(item1);
        DataSource.Dependeencies.Add(item);

    }
}
