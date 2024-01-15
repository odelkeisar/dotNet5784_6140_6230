using DO;

namespace DalApi;

public interface IChef
{
    int Create(Chef item); //Creates new entity object in DAL
    Chef? Read(int id); //Reads entity object by its ID 

    public Chef? Read(Func<Chef, bool> filter);
    public IEnumerable<Chef> ReadAll(Func<Chef, bool>? filter = null); //stage 1 only, Reads all entity objects
    void Update(Chef item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id

}
