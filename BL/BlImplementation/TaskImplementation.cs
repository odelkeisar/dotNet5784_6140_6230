using BlApi;
using BO;
using DO;



namespace BlImplementation;
internal class TaskImplementation : ITask1
{
    private DalApi.IDal _dal = Factory.Get;
    public int Create(BO.Task1 item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    { 
        BO.Task1 ?task=Read(id);

        if((_dal.Dependeency.ReadAll(x=>x.DependsOnTask==id))!=null)
            throw new BlATaskCannotBeDeletedException($"The task cannot be deleted:{id} The task has tasks that depend on it");
        if(task.StartDate!=null)
            throw new BlATaskCannotBeDeletedException($"The task:{id} already in the process of execution and cannot be deleted");

        try { _dal.Task1.Delete(id); }
        catch(DalDoesNotExistException ex) { throw new BlDoesNotExistException($"Task with ID={id} does not exist", ex); };
    }

    public BO.Task1? Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.Task1> ReadAll(Func<DO.Task1, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Task1 item)
    {
        if(item.
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            )
    }

    public void UpdateScheduledDate(int id, DateTime scheduledDate)
    {
        DO.Task1? task = _dal.Task1.Read(id);
        if (task == null)
            throw new BlDoesNotExistException($"Task with ID={id} does not exists");  //איתור המשימה הנדרשת

        IEnumerable<BO.TaskInList>? listDependeencies = Read(id)!.dependeencies;  //יצירת רשימת תלויות של כל המשימות שהמשימה תלויה בהם  


        //foreach (var taskinlist in listDependeencies)     //מעבר על כל משימה קודמת ובדיקה שתאריך ההתחלה המתוכנן קיים וגם שהתאריך שהתקבל כפרמטר אינו מוקדם מתאריך הסיום המשוער של כל משימה שקודמת לה 
        //{
        //    BO.Task1 task_ = Read(taskinlist.Id)!;
        //    if (task_.ScheduledDate == null)
        //        throw new BlScheduledStartDateNoUpdatedException($"Scheduled start date of previous mission: {taskinlist.Id}, not updated");
        //    if (task_.ForecastDate > scheduledDate)
        //        throw new BlEarlyFinishDateFromPreviousTaskException($"It is not possible to update an end date for task ID:{id} earlier than the end date of a previous task ID:{task_.Id}");
        //}

        DateTime? scheduledDate_ = Read(listDependeencies!.FirstOrDefault()!.Id)!.ScheduledDate;

        foreach (var taskinlist in listDependeencies!)     //מעבר על כל משימה קודמת ובדיקה שתאריך ההתחלה המתוכנן קיים וגם שהתאריך שהתקבל כפרמטר אינו מוקדם מתאריך הסיום המשוער של כל משימה שקודמת לה 
        {
            BO.Task1 task_ = Read(taskinlist.Id)!;
            if (task_.ScheduledDate == null)
                throw new BlScheduledStartDateNoUpdatedException($"Scheduled start date of previous mission: {taskinlist.Id}, not updated");
            if (task_.ForecastDate > scheduledDate_)
                scheduledDate_ = task_.ForecastDate;
        }

        //שליחת תאריך 
        try
        { _dal.Task1.Update(new DO.Task1(task.Id, task.Alias, task.Description, task.CreatedAtDate, scheduledDate, task.RequiredEffortTime, task.DeadlineDate, task.ChefId, task.StartDate, task.CompleteDate, task.Copmlexity, task.Dellverables, task.Remarks, task.isMileStone)); }

        catch (DalDoesNotExistException ex)
        { throw new BlDoesNotExistException($"Task with ID={id} does not exists", ex); }

    }
}

