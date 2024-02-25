namespace BlImplementation;
using BlApi;

/// <summary>
/// Implementation of Ibl class
/// </summary>
internal class Bl : IBl
{
    public ITask1 Task1 => new TaskImplementation();

    public IChef Chef => new ChefImplementation();

    public void InitializeDB() => DalTest.Initialization.Do();
    public void InitializeResetB() => DalTest.Initialization.Reset();
 



}