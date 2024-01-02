using DO;

namespace DalApi;

public interface ITask
{
    int Create(DO.Task item); //Creates new entity object in DAL
    DO.Task? Read(int id); //Reads entity object by its ID 
    List<DO.Task> ReadAll(); //stage 1 only, Reads all entity objects
    void Update(DO.Task item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id

}