using DalApi;

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
    static internal Status GetStatus(DO.Task1 task)
    {
        if (task.CompleteDate != null)
            return Status.בוצע;

        if (task.StartDate != null)
            return Status.בתהליך;

        if (task.ScheduledDate != null)
            return Status.מתוזמן;

        return Status.בלתי_מתוכנן;
    }


}

    
