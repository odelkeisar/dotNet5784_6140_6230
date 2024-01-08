using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;
using DalApi;
sealed public class DalList : IDal
{

    public ITask1 Task1 => new TaskImplementation();
    public IEngineer Engineer => new EngineerImplementation();
    public IDependeency Dependeency => new DependeencyImplementation();
   
}


