using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;
using DalApi;
/// <DalList>
/// A class inherits from IDeal and implements it
/// </summary>
sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();
    private DalList() { }

    public ITask1 Task1 => new TaskImplementation();
    public IChef Chef => new ChefImplementation();
    public IDependeency Dependeency => new DependeencyImplementation();

}


