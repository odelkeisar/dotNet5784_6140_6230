using BlApi;
using BO;
using System.Linq;
using DO;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace BlImplementation;
internal class TaskImplementation : ITask1
{
    private DalApi.IDal _dal = Factory.Get;

    /// <summary>
    /// create a new task
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="BlWrongNegativeIdException"> if the id is negative throw experation</exception>
    /// <exception cref="BlEmptyStringException">if have empty string</exception>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>

    public DateTime? ReadStartProject()
    {
        return _dal.Task1.ReadStartProject();
    }

    public DateTime? ReadEndProject()
    {
        return _dal.Task1.ReadEndProject();
    }
    public void CreateStartProject(DateTime starProject)
    {
        IEnumerable<DO.Task1?>? tasks = _dal.Task1.ReadAll();

        if (tasks!.Any(task => task!.ScheduledDate < starProject))
            throw new BlWrongDateException("An earlier date must be entered for the project");

        _dal.Task1.UpdateStartProject(starProject);
    }

    public void CreateEndProject(DateTime endProject)
    {
        IEnumerable<DO.Task1?>? tasks = _dal.Task1.ReadAll();
        if (tasks!.Any(task => task!.ScheduledDate == null))
            throw new BlScheduledStartDateNoUpdatedException("Not all missions have an updated scheduled start date yet.");

        if (tasks!.Any(task => (task!.ScheduledDate + task.RequiredEffortTime) < endProject))
            throw new BlWrongDateException("A later end date must be entered for the project");

        _dal.Task1.UpdateEndProject(endProject);
    }


    public int Create(BO.Task1 item)
    {
        if (_dal.Task1.ReadEndProject() != null)
            throw new BlInappropriateStepException("It is not possible to add a task after the schedule has been set");
        if (item.Id < 0)
            throw new BlWrongNegativeIdException("Task with negative ID");
        if (item.Alias == "")
            throw new BlEmptyStringException("The string is empty");
        if (item.chef != null)
            throw new BlInappropriateStepException("A chef cannot be assigned before the schedule is set");

        DO.Task1 doTask = new DO.Task1
       (0, item.Alias, item.Description, item.CreatedAtDate, item.ScheduledDate, item.RequiredEffortTime,
       item.DeadlineDate, item.chef == null ? 0 : item.chef.Id, item.StartDate, item.CompleteDate, (DO.ChefExperience)item.Copmlexity!,
       item.Dellverables, item.Remarks, null);

        try
        {
            int idTask1 = _dal.Task1.Create(doTask);
            IEnumerable<BO.TaskInList> taskInList = item.dependeencies!;

            IEnumerable<DO.Dependeency> dependeenciesList = from _item in taskInList
                                                            select new DO.Dependeency { Id = 0, DependentTask = idTask1, DependsOnTask = _item.Id };

            foreach (var dependeency in dependeenciesList)
            {
                _dal.Dependeency.Create(dependeency);
            }
            return idTask1;
        }

        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={item.Id} already exists", ex);
        }
    }

    /// <summary>
    /// Deleting a task received as a parameter
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BlATaskCannotBeDeletedException"></exception>
    /// <exception cref="BlDoesNotExistException"></exception>

    public void Delete(int id)
    {
        if (_dal.Task1.ReadEndProject != null)
            throw new BlInappropriateStepException("It is not possible to delete a task after the schedule has been set");

        BO.Task1? task = Read(id);

        if (task == null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");
        if ((_dal.Dependeency.ReadAll(x => x.DependsOnTask == id)) != null)
            throw new BlATaskCannotBeDeletedException($"The task cannot be deleted:{id} The task has tasks that depend on it");
        if (task.StartDate != null)
            throw new BlATaskCannotBeDeletedException($"The task:{id} already in the process of execution and cannot be deleted");

        try { _dal.Task1.Delete(id); }
        catch (DalDoesNotExistException ex) { throw new BlDoesNotExistException($"Task with ID={id} does not exist", ex); };
    }

    /// <summary>
    /// You get a mission ID card and return its details
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public BO.Task1? Read(int id)
    {
        DO.Task1? doTask = _dal.Task1.Read(id);

        if (doTask == null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");

        BO.Task1? boTask = convert(doTask!);
        return boTask;
    }
    /// <summary>
    /// Returning all task details.
    /// </summary>
    /// <returns></returns>

    public IEnumerable<BO.TaskInList> ReadAll()
    {
        return (from DO.Task1 doTask in _dal.Task1.ReadAll()!
                select new BO.TaskInList() { Id = doTask.Id, Description = doTask.Description, Alias = doTask.Alias, status = Tools.GetStatus(doTask) });
    }

    /// <summary>
    /// Returning all the tasks that match the chef that was passed as a parameter.
    /// </summary>
    /// <param name="chef"></param>
    /// <returns></returns>
    /// <exception cref="BlNoTasksToCompleteException"></exception>

    public IEnumerable<BO.TaskInList> ReadAllPossibleTasks(BO.Chef chef)
    {
        List<BO.Task1>? possibleTasks = new List<BO.Task1>();
        IEnumerable<BO.Task1>? allTasks = _dal.Task1.ReadAll(_dotask => _dotask.ScheduledDate != null)!.Select(_dotask => convert(_dotask!))!;
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
        return (possibleTasks.Select(boTask => new BO.TaskInList() { Id = boTask.Id, Description = boTask.Description, Alias = boTask.Alias, status = boTask.status }));
    }


    /// <summary>
    /// Update an existing task
    /// </summary>
    /// <param name="item">the new task</param>
    /// <exception cref="BlWrongNegativeIdException"></exception>
    /// <exception cref="BlEmptyStringException"></exception>
    /// <exception cref="BlChefLevelTooLowException"></exception>
    public void Update(BO.Task1 item)
    {
        BO.Task1? botask = Read(item.Id); //בדיקה שהמשימה קיימת

        if (botask == null)
            throw new BlDoesNotExistException($"Task with ID={item.Id} does not exists");

        if (item.Alias == "")
            throw new BlEmptyStringException("The string is empty");

        if (ReadEndProject == null)
        {
            if (item.chef != null)
                throw new BlInappropriateStepException("A chef cannot be assigned to a task before the schedule is set");
            if (item.StartDate != null)
                throw new BlInappropriateStepException("It is not possible to update an actual start date before the schedule is set");
            if (item.CompleteDate != null)
                throw new BlInappropriateStepException("It is not possible to update an actual complete date before the schedule is set");
            if (item.ScheduledDate != null)
            {
                try
                {
                    UpdateScheduledDate(item.Id, (DateTime)item.ScheduledDate);
                }

                catch (BlDoesNotExistException ex)
                {
                    throw new BlDoesNotExistException(ex.Message);
                }
                catch (BlScheduledStartDateNoUpdatedException ex)
                {
                    throw new BlScheduledStartDateNoUpdatedException(ex.Message);
                }
                catch (BlEarlyFinishDateFromPreviousTaskException ex)
                {
                    throw new BlEarlyFinishDateFromPreviousTaskException(ex.Message);
                }
            }

        }

        else
        {
            if (item.ScheduledDate != botask.ScheduledDate)
                throw new BlInappropriateStepException("It is not possible to change a planned start date after the schedule has been set");

            if (item.chef == null && botask.chef != null)
                throw new BlTaskAlreadyAssignedException("It is not possible to cancel a chef that already exists in the mission");
            if (item.CompleteDate != null && item.StartDate == null)
                throw new BlWrongDateException("You cannot enter an actual end date before an actual start date");

            if (item.chef != null)
            {
                if (item.Copmlexity == null)
                    throw new BllackingInLevelException("In order to associate a chef, complexity must be entered");

                DO.Chef? _chef = _dal.Chef.Read((int)item.chef.Id!);
                if (_chef == null)
                    throw new BlDoesNotExistException($"Chef with ID={item.Id} does not exists");

                DO.Task1? dotask = _dal.Task1.Read(task => task.ChefId == item.chef.Id); //חיפוש המשימה שהשף כבר מוקצה לה
                if (dotask != null && dotask.Id != item.Id && dotask.CompleteDate == null)   // אם השף כבר מוקצה למשימה שאינה זהה למשימה החדשה המעודכנת וגם המשימה הקודמת טרם הושלמה
                    throw new BlNoChangeChefAssignmentException($"The chef with{item.Id} is already assigned to an unfinished task");

                if (botask.chef != null && botask.chef.Id != item.chef.Id)   // לא ניתן להקצות משימה לשף אם המשימה כבר מוקצית לשף אחר
                    throw new BlTaskAlreadyAssignedException($"The task with the ID{botask.Id} is already assigned to the chef with the ID {botask.chef.Id}");

                if (item.Copmlexity > (BO.ChefExperience)_chef.Level!) //בדיקה שרמת השף הנדרשת מתאימה לשף המתעדכן
                    throw new BlChefLevelTooLowException("The level of the engineer is lower than the complexity of the task");
            }

        }

        _dal.Task1.Update(new DO.Task1(item.Id, item.Alias, item.Description, item.CreatedAtDate, item.ScheduledDate,
                          item.RequiredEffortTime, item.DeadlineDate, item.chef == null ? 0 : item.chef.Id,
                          item.StartDate, item.CompleteDate, (DO.ChefExperience)item.Copmlexity!, item.Dellverables,
                          item.Remarks, null));
    }

    /// <summary>
    /// Update scheduled start date for task.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="scheduledDate"></param>
    /// <exception cref="BlDoesNotExistException"></exception>
    /// <exception cref="BlScheduledStartDateNoUpdatedException"></exception>
    public void UpdateScheduledDate(int id, DateTime scheduledDate)
    {
        DO.Task1? task = _dal.Task1.Read(id);
        if (task == null)
            throw new BlDoesNotExistException($"Task with ID={id} does not exists");  //איתור המשימה הנדרשת

        IEnumerable<BO.TaskInList>? listDependeencies = Read(id)!.dependeencies;  //יצירת רשימת תלויות של כל המשימות שהמשימה תלויה בהם  
        if (listDependeencies != null)
        {
            foreach (var taskinlist in listDependeencies)     //מעבר על כל משימה קודמת ובדיקה שתאריך ההתחלה המתוכנן קיים וגם שהתאריך שהתקבל כפרמטר אינו מוקדם מתאריך הסיום המשוער של כל משימה שקודמת לה 
            {
                BO.Task1 task_ = Read(taskinlist.Id)!;
                if (task_.ScheduledDate == null)
                    throw new BlScheduledStartDateNoUpdatedException($"Scheduled start date of previous mission: {taskinlist.Id}, not updated");
                if (task_.ForecastDate > scheduledDate)
                    throw new BlEarlyFinishDateFromPreviousTaskException($"It is not possible to update an end date for task ID:{id} earlier than the end date of a previous task ID:{task_.Id}");
            }
        }

        //DateTime? scheduledDate_ = Read(listDependeencies!.FirstOrDefault()!.Id)!.ScheduledDate;

        //foreach (var taskinlist in listDependeencies!)     //מעבר על כל משימה קודמת ובדיקה שתאריך ההתחלה המתוכנן קיים וגם שהתאריך שהתקבל כפרמטר אינו מוקדם מתאריך הסיום המשוער של כל משימה שקודמת לה 
        //{
        //    BO.Task1 task_ = Read(taskinlist.Id)!;
        //    if (task_.ScheduledDate == null)
        //        throw new BlScheduledStartDateNoUpdatedException($"Scheduled start date of previous mission: {taskinlist.Id}, not updated");
        //    if (task_.ForecastDate > scheduledDate_)
        //        scheduledDate_ = task_.ForecastDate;
        //}

        //שליחת תאריך 
        try
        { _dal.Task1.Update(new DO.Task1(task.Id, task.Alias, task.Description, task.CreatedAtDate, scheduledDate, task.RequiredEffortTime, task.DeadlineDate, task.ChefId, task.StartDate, task.CompleteDate, task.Copmlexity, task.Dellverables, task.Remarks, task.isMileStone)); }

        catch (DalDoesNotExistException ex)
        { throw new BlDoesNotExistException($"Task with ID={id} does not exists", ex); }

    }

    /// <summary>
    /// Returning all tasks matching the level of the chef that the function received as a parameter.
    /// </summary>
    /// <param name="chef"></param>
    /// <returns></returns>
    public IEnumerable<BO.TaskInList> ReadAllPerLevelOfChef(BO.Chef chef)
    {
        IEnumerable<BO.TaskInList> listTasks = _dal.Task1.ReadAll()!
      .Where(doTask => doTask!.Copmlexity == (DO.ChefExperience)chef.Level!)
      .Select(doTask => new BO.TaskInList() { Id = doTask!.Id, Description = doTask.Description, Alias = doTask.Alias, status = Tools.GetStatus(doTask) });
        if (listTasks == null)
            throw new BlNoTasksbyCriterionException("There are no tasks that correspond to the chef level");
        return listTasks;
    }

    /// <summary>
    /// Returning all tasks that match a certain level.
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>

    public IEnumerable<BO.TaskInList> ReadAllPerLevel(BO.ChefExperience _level)
    {
        IEnumerable<DO.Task1?>? tasks = _dal.Task1.ReadAll(doTask => doTask!.Copmlexity == (DO.ChefExperience)_level);
        if (tasks == null)
            throw new BlDoesNotExistException("No tasks per level");

        return (tasks.Select(doTask => new BO.TaskInList() { Id = doTask!.Id, Description = doTask.Description, Alias = doTask.Alias, status = Tools.GetStatus(doTask) }));
    }
    /// <summary>
    /// Returning all tasks that have already been completed.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="BlDoesNotExistException"></exception>
    public IEnumerable<BO.TaskInList> ReadAllCompleted()
    {
        IEnumerable<DO.Task1?>? tasks = _dal.Task1.ReadAll(task => task.CompleteDate != null);
        if (tasks == null)
            throw new BlDoesNotExistException("No tasks completed");
        return tasks.Select(doTask => new BO.TaskInList() { Id = doTask!.Id, Description = doTask.Description, Alias = doTask.Alias, status = Tools.GetStatus(doTask) });
    }

    /// <summary>
    /// Returning all the tasks that are in care.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="BlDoesNotExistException"></exception>

    public IEnumerable<BO.TaskInList> ReadAllTasksInCare()
    {
        IEnumerable<DO.Task1?>? tasks = _dal.Task1.ReadAll(task => task.StartDate != null);
        if (tasks == null)
            throw new BlDoesNotExistException("There are no tasks currently being handled by Chef");
        return tasks.Select(doTask => new BO.TaskInList() { Id = doTask!.Id, Description = doTask.Description, Alias = doTask.Alias, status = Tools.GetStatus(doTask) });
    }

    /// <summary>
    /// Returning all tasks for which no chef is assigned.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="BlDoesNotExistException"></exception>

    public IEnumerable<BO.TaskInList> ReadAllNoChefWasAssigned()
    {
        IEnumerable<DO.Task1?>? tasks = _dal.Task1.ReadAll(task => task.ChefId == 0);
        if (tasks == null)
            throw new BlDoesNotExistException("All tasks are assigned to chefs");
        return tasks.Select(doTask => new BO.TaskInList() { Id = doTask!.Id, Description = doTask.Description, Alias = doTask.Alias, status = Tools.GetStatus(doTask) });
    }

    /// <summary>
    /// Returning all tasks that do not have a scheduled start date.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="BlDoesNotExistException"></exception>
    public IEnumerable<BO.TaskInList> ReadAllNoScheduledDate()
    {
        IEnumerable<DO.Task1?>? tasks = _dal.Task1.ReadAll(task => task.ScheduledDate == null);
        if (tasks == null)
            throw new BlDoesNotExistException("All tasks have a scheduled start date");
        return tasks.Select(doTask => new BO.TaskInList() { Id = doTask!.Id, Description = doTask.Description, Alias = doTask.Alias, status = Tools.GetStatus(doTask) });
    }

    /// <summary>
    /// Returning the list of all tasks that the current task depends on and converting them to an object of type:TaskInList.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<TaskInList>? GetTaskInList(int id)
    {
        IEnumerable<DO.Dependeency>? listDependencies = _dal.Dependeency.ReadAll(X => X.DependentTask == id)!;

        List<BO.TaskInList>? results = new List<BO.TaskInList>();

        var v = from item in listDependencies
                select (_dal.Task1.Read(item.DependsOnTask));

        foreach (var task in v)
        {
            results.Add(new BO.TaskInList() { Id = task.Id, Alias = task.Alias, Description = task.Description, status = Tools.GetStatus(task) });
        }
        return results;
    }


    /// <summary>
    /// Converting an object of type DO.Task 1 to an object of type BO.Task1.
    /// </summary>
    /// <param name="doTask"></param>
    /// <returns></returns>
    public BO.Task1 convert(DO.Task1 doTask)
    {
        return new BO.Task1()
        {
            Id = doTask!.Id,
            Alias = doTask.Alias,
            Description = doTask.Description,
            status = Tools.GetStatus(doTask),
            dependeencies = GetTaskInList(doTask.Id),
            CreatedAtDate = doTask.CreatedAtDate,
            ScheduledDate = doTask.ScheduledDate,
            StartDate = doTask.StartDate,
            DeadlineDate = doTask.DeadlineDate,
            CompleteDate = doTask.CompleteDate,
            RequiredEffortTime = doTask.RequiredEffortTime,
            Dellverables = doTask.Dellverables,
            Remarks = doTask.Remarks,
            chef = doTask.ChefId == 0 ? null : new ChefInTask { Id = (int)doTask.ChefId!, Name = _dal.Chef.Read((int)doTask.ChefId)!.Name },
            Copmlexity = doTask.Copmlexity != null ? (BO.ChefExperience)doTask.Copmlexity : null
        };
    }

}




