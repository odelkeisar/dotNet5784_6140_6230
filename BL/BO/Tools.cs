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
            return Status.Done;

        if (task.ChefId != 0)
            return Status.OnTrack;

        if (task.ScheduledDate != null)
            return Status.Scheduled;

        return Status.Unscheduled;
    }


}

    
