using BlImplementation;
using DalApi;
using System.Reflection;

namespace BO;
/// <summary>
/// class of help function
/// </summary>
static public class Tools
{

    /// <summary>
    /// Returning the task status field according to criteria.
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    static internal Status GetStatus(this TaskImplementation task1, DO.Task1 task)
    {
        if (task.CompleteDate != null)
            return Status.בוצע;

        if (task.StartDate != null)
            return Status.בתהליך;

        if (task.ScheduledDate != null)
            return Status.מתוזמן;

        return Status.בלתי_מתוכנן;
    }
    public static string ToStringProperty<T>(this T t)
    {
        string str = "";
        foreach (PropertyInfo item in t.GetType().GetProperties()) //ריצה על כל התכונות של הישות 
            str += "\n" + item.Name  + ": " + item.GetValue(t, null);
        return str;
    }
}