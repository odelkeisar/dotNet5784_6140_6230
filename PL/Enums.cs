using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL;

    internal class LevelChef : IEnumerable
    {
        static readonly IEnumerable<BO.ChefExperience> c_enums =
    (Enum.GetValues(typeof(BO.ChefExperience)) as IEnumerable<BO.ChefExperience>)!;

        public IEnumerator GetEnumerator() => c_enums.GetEnumerator();
    }

