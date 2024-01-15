using DalApi;

namespace Dal;


//stage 3

sealed public class DalXml : IDal
{
    public ITask1 Task1 => new TaskImplementation();

    public IChef Chef =>  new ChefImplementation();

    public IDependeency Dependeency => new DependeencyImplementation();
}
