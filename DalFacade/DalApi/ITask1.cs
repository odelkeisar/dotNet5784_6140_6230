using DO;

namespace DalApi;

public interface ITask1
{
    int Create(Task1 item); //Creates new entity object in DAL
    Task1? Read(int id); //Reads entity object by its ID 
    List<Task1> ReadAll(); //stage 1 only, Reads all entity objects
    void Update(Task1 item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id
}

