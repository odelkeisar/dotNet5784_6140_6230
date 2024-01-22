using DalApi;
using System.Diagnostics;

namespace Dal;
/// <summary>
/// A class inherits from IDeal and implements it
/// </summary>

//stage 3

sealed public class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }
    public ITask1 Task1 => new TaskImplementation();

    public IChef Chef =>  new ChefImplementation();

    public IDependeency Dependeency => new DependeencyImplementation();
}
