using BlApi;
using BO;
using DO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace BlImplementation;
internal class TaskImplementation : ITask1
{
    private DalApi.IDal _dal = Factory.Get;
    public int Create(BO.Task1 item)
    {
        if (item.Id < 0)
            throw new BlWrongNegativeIdException("Task with negative ID");
        if (item.Alias == "")
            throw new BlEmptyStringException("The string is empty");

        IEnumerable<BO.TaskInList> tasks = item.dependeencies!;

        var dependeenciesList = from _item in tasks
                                select new DO.Dependeency { Id = 0, DependentTask = item.Id, DependsOnTask = _item.Id };

        foreach (var dependeency in dependeenciesList)
        {
            _dal.Dependeency.Create(dependeency);
        }

        DO.Task1 doTask = new DO.Task1
       (0, item.Alias, item.Description, item.CreatedAtDate, item.ScheduledDate, item.RequiredEffortTime,
       item.DeadlineDate, item.chef == null ? 0 : item.chef.Id, item.StartDate, item.CompleteDate, (DO.ChefExperience)item.Copmlexity!,
       item.Dellverables, item.Remarks, null);

        try
        {
            int idTask1 = _dal.Task1.Create(doTask);
            return idTask1;
        }

        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={item.Id} already exists", ex);
        }
    }

    public void Delete(int id)
    {
        BO.Task1? task = Read(id);

        if ((_dal.Dependeency.ReadAll(x => x.DependsOnTask == id)) != null)
            throw new BlATaskCannotBeDeletedException($"The task cannot be deleted:{id} The task has tasks that depend on it");
        if (task.StartDate != null)
            throw new BlATaskCannotBeDeletedException($"The task:{id} already in the process of execution and cannot be deleted");

        try { _dal.Task1.Delete(id); }
        catch (DalDoesNotExistException ex) { throw new BlDoesNotExistException($"Task with ID={id} does not exist", ex); };
    }

    public BO.Task1? Read(int id)
    {
        DO.Task1? doTask = _dal.Task1.Read(id);

        if (doTask == null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");

        BO.Task1? task_ = new BO.Task1()
        {
            Id = id,
            Alias = doTask.Alias,
            Description = doTask.Description,
            status = GetStatus(doTask),
            dependeencies = GetTaskInList(id),
            CreatedAtDate = doTask.CreatedAtDate,
            ScheduledDate = doTask.ScheduledDate,
            StartDate = doTask.StartDate,
            DeadlineDate = doTask.DeadlineDate,
            CompleteDate = doTask.CompleteDate,
            RequiredEffortTime = doTask.RequiredEffortTime,
            Dellverables = doTask.Dellverables,
            Remarks = doTask.Remarks,
            chef = new ChefInTask { Id = doTask.ChefId, Name = _dal.Chef.Read(doTask.ChefId)!.Name },
            Copmlexity = (BO.ChefExperience)doTask.Copmlexity!
        };
        return task_;
    }

    public IEnumerable<BO.Task1>? ReadAll()
    {
        return (from DO.Task1 doTask in _dal.Task1.ReadAll()!
                select new BO.Task1
                {
                    Id = doTask!.Id,
                    Alias = doTask.Alias,
                    Description = doTask.Description,
                    status = GetStatus(doTask),
                    dependeencies = GetTaskInList(doTask.Id),
                    CreatedAtDate = doTask.CreatedAtDate,
                    ScheduledDate = doTask.ScheduledDate,
                    StartDate = doTask.StartDate,
                    DeadlineDate = doTask.DeadlineDate,
                    CompleteDate = doTask.CompleteDate,
                    RequiredEffortTime = doTask.RequiredEffortTime,
                    Dellverables = doTask.Dellverables,
                    Remarks = doTask.Remarks,
                    chef = new ChefInTask { Id = doTask.ChefId, Name = _dal.Chef.Read(doTask.ChefId)!.Name },
                    Copmlexity = (BO.ChefExperience)doTask.Copmlexity!
                });
    }
    public IEnumerable<BO.Task1>? ReadAllPossibleTasks(BO.Chef chef)
    {
        List<BO.Task1> ?possibleTasks = new List<BO.Task1>();
        IEnumerable<BO.Task1>? allTasks = ReadAll()!;
        // קיבוץ המשימות לפי רמת הקושי שלהם
        var groupedTasksByComplexity = allTasks.GroupBy(t => t.Copmlexity);

        // עבור כל קבוצת משימות לפי רמת הקושי
        foreach (var group in groupedTasksByComplexity)
        {
            // בדיקה האם רמת הקושי של הקבוצה גבוהה מרמת השף
            if (group.Key <= chef.Level)
            {
                // הוספת כל המשימות של רמת הקושי הנוכחית לרשימת המשימות האפשריות
                possibleTasks.AddRange(group.Where(x => x.ScheduledDate != null));
                possibleTasks.AddRange(group.Where(x => x.dependeencies == null ? true : x.dependeencies.All(dependency => dependency.status == Status.Done)));
            }
        }
        if (possibleTasks == null)
            throw new BlNoTasksToCompleteException($"There are no possible tasks to perform for the chef:{chef.Id}");
        return possibleTasks;
    }



    public void Update(BO.Task1 item)
    {

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

    public IEnumerable<BO.Task1>? ReadAllPerLevel(BO.Chef chef)
    {
        return _dal.Task1.ReadAll()!
      .Where(doTask => doTask!.Copmlexity == (DO.ChefExperience)chef.Level!)
      .Select(doTask => new BO.Task1()
      {
          Id = doTask!.Id,
          Alias = doTask.Alias,
          Description = doTask.Description,
          status = GetStatus(doTask),
          dependeencies = GetTaskInList(doTask.Id),
          CreatedAtDate = doTask.CreatedAtDate,
          ScheduledDate = doTask.ScheduledDate,
          StartDate = doTask.StartDate,
          DeadlineDate = doTask.DeadlineDate,
          CompleteDate = doTask.CompleteDate,
          RequiredEffortTime = doTask.RequiredEffortTime,
          Dellverables = doTask.Dellverables,
          Remarks = doTask.Remarks,
          chef = new ChefInTask { Id = doTask.ChefId, Name = _dal.Chef.Read(doTask.ChefId)!.Name },
          Copmlexity = (BO.ChefExperience)doTask.Copmlexity!

      });
    }
    public Status GetStatus(DO.Task1 task)
    {
        if (task.CompleteDate != null)
            return Status.Done;

        if (task.ChefId != 0)
            return Status.OnTrack;

        if (task.ScheduledDate != null)
            return Status.Scheduled;

        return Status.Unscheduled;
    }

    public List<TaskInList>? GetTaskInList(int id)
    {
        IEnumerable<DO.Dependeency>? listDependencies = _dal.Dependeency.ReadAll(X => X.DependentTask == id)!;
        var results = listDependencies.Select(dependency => _dal.Task1.Read(dependency.DependsOnTask)).
            Select(X => new TaskInList() { Id = X.Id, Alias = X.Alias, Description = X.Description, status = GetStatus(X) });
        return results.ToList();
    }
}



















