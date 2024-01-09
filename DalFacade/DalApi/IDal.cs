using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi;
/// <sumery>
/// An interface with an attribute for each type of subinterface
/// </summary>
public interface IDal
{
    ITask1 Task1 { get; }
    IChef Chef { get; }
    IDependeency Dependeency { get; }
}

