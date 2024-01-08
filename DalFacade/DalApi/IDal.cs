using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi;
public interface IDal
{
    ITask1 Task1 { get; }
    IEngineer Engineer { get; }
    IDependeency Dependeency { get; }
}

