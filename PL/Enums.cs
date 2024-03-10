using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL;
/// <summary>
///לchefExperience ienumerator ומספקת ienumerable מחלקה פנימית הממשת את הממשק 
/// </summary>
internal class LevelChef : IEnumerable
{
    static readonly IEnumerable<BO.ChefExperience> c_enums = //אוסף של כל הערכים האפשריים באינם
(Enum.GetValues(typeof(BO.ChefExperience)) as IEnumerable<BO.ChefExperience>)!;

    public IEnumerator GetEnumerator() => c_enums.GetEnumerator();
}


internal class StatusTask : IEnumerable
{
    static readonly IEnumerable<BO.Status> c_enums =
(Enum.GetValues(typeof(BO.Status)) as IEnumerable<BO.Status>)!;

    public IEnumerator GetEnumerator() => c_enums.GetEnumerator();
}


