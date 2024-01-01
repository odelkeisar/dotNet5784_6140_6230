using DO;

namespace DalApi;
public interface IDependeency
{
    int Create(Dependeency item); //Creates new entity object in DAL
    Dependeency? Read(int id); //Reads entity object by its ID 
    List<Dependeency> ReadAll(); //stage 1 only, Reads all entity objects
    void Update(Dependeency  item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id

}
