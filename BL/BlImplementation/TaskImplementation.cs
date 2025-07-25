﻿using BlApi;
using BO;
using System.Linq;
using DO;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Data;
using System.Collections.Generic;
using System.Numerics;



namespace BlImplementation;
internal class TaskImplementation : ITask1
{
    private DalApi.IDal _dal = Factory.Get;
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    /// <summary>
    /// the function return date to start project
    /// </summary>
    /// <returns></returns>

    public DateTime? ReadStartProject()
    {
        return _dal.Task1.ReadStartProject();
    }

    /// <summary>
    /// the function return date to end project
    /// </summary>
    /// <returns></returns>
    public DateTime? ReadEndProject()
    {
        return _dal.Task1.ReadEndProject();
    }

    /// <summary>
    /// the function put date to start project
    /// </summary>
    /// <param name="starProject"></param>
    /// <exception cref="BlNullPropertyException"></exception>
    /// <exception cref="BlWrongDateException"></exception>
    public void CreateStartProject(DateTime? starProject)
    {
        if (starProject == null)
            throw new BlNullPropertyException("נא להכניס תאריך");
        IEnumerable<DO.Task1?>? tasks = _dal.Task1.ReadAll();

        if (tasks!.Any(task => task!.ScheduledDate < starProject))
            throw new BlWrongDateException("יש להזין תאריך מוקדם יותר מהתאריך המתוכנן להתחלה של המשימה הראשונה");

        _dal.Task1.UpdateStartProject((DateTime)starProject);
    }

    /// <summary>
    ///  the function put date to end project
    /// </summary>
    /// <param name="endProject"></param>
    /// <exception cref="BlNullPropertyException"></exception>
    /// <exception cref="BlScheduledStartDateNoUpdatedException"></exception>
    /// <exception cref="BlWrongDateException"></exception>
    public void CreateEndProject(DateTime? endProject)
    {
        if (endProject == null)
            throw new BlNullPropertyException("נא להכניס תאריך");

        IEnumerable<DO.Task1?>? tasks = _dal.Task1.ReadAll();
        if (tasks!.Any(task => task!.ScheduledDate == null))
            throw new BlScheduledStartDateNoUpdatedException("יש להזין קודם את כל התאריכים המתוכננים להתחלה של כל המשימות");

        if (tasks!.Any(task => (task!.ScheduledDate + task.RequiredEffortTime) > endProject))
            throw new BlWrongDateException("יש להזין תאריך מאוחר יותר מהתאריך המתוכנן לסיום של המשימה האחרונה");

        _dal.Task1.UpdateEndProject((DateTime)endProject);
    }

    /// <summary>
    /// create a new task
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="BlWrongNegativeIdException"> if the id is negative throw experation</exception>
    /// <exception cref="BlEmptyStringException">if have empty string</exception>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>

    public int Create(BO.Task1 item)
    {
        if (_dal.Task1.ReadEndProject() != null)
            throw new BlInappropriateStepException("לא ניתן להוסיף משימה לאחר קביעת הלוז");
        if (item.Id < 0)
            throw new BlWrongNegativeIdException("המשימה עם מספר תעודת זהות שלילית");
        if (item.Alias == "")
            throw new BlEmptyStringException("המחרוזת ריקה");
        if (item.chef != null)
            throw new BlInappropriateStepException("לא ניתן להקצות שף לפני קביעת הלוז");
        if (item.RequiredEffortTime == null)
            throw new BlProblemAboutRequiredEffortTimeException("יש להזין משך זמן משימה");
        if (item.Copmlexity == BO.ChefExperience.ללא_סינון || item.Copmlexity == null)
            throw new BlChefLevelNoEnteredException("יש להזין רמת קושי");
        if (item.ScheduledDate != null)
        {
            if (item.ScheduledDate < ReadStartProject())
                throw new BlWrongDateException("אי אפשר לקבוע תאריך מתוכנן להתחלה מוקדם יותר מתאריך תחילת הפרויקט ");

            foreach (var task in item.dependeencies!)
            {
                BO.Task1 _task = s_bl.Task1.Read(task.Id)!;

                if (_task.ForecastDate > item.ScheduledDate)
                    throw new BlWrongDateException("אי אפשר לקבוע תאריך מתוכנן להתחלה מוקדם יותר מהתאריך המשואר לסיום של משימות קודמות ");
            }
        }

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
            throw new BO.BlAlreadyExistsException($"המשימה עם התעודת זהות{item.Id} כבר קיימת", ex);
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
        if (_dal.Task1.ReadEndProject() != null)
            throw new BlInappropriateStepException("לא ניתן למחוק משימה לאחר קביעת הלוז");

        BO.Task1? task = Read(id);

        if (task == null)
            throw new BO.BlDoesNotExistException($"המשימה עם התעודת זהות{id} לא קיימת");
        if ((_dal.Dependeency.ReadAll(x => x.DependsOnTask == id)!.Count() != 0))
            throw new BlATaskCannotBeDeletedException($"לא ניתן למחוק את המשימה:{id} למשימה יש משימות התלויות בה");
        if (task.StartDate != null)
            throw new BlATaskCannotBeDeletedException($"המשימה:{id} כבר באמצע הביצוע ולכן לא ניתן למחוק אותה");

        try
        {
            var depend = _dal.Dependeency.ReadAll(X => X.DependentTask == id);

            foreach (var item in depend!)
            {
                _dal.Dependeency.Delete(item!.Id);
            }

            _dal.Task1.Delete(id);
        }
        catch (DalDoesNotExistException ex) { throw new BlDoesNotExistException($"המשימה עם התעודת זהות {id} לא קיימת", ex); };
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
            throw new BO.BlDoesNotExistException($"המשימה עם התעודת זהות{id} לא קיימת");

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
                select new BO.TaskInList() { Id = doTask.Id, Description = doTask.Description, Alias = doTask.Alias, status = this.GetStatus(doTask) });
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
                possibleTasks.AddRange(group.Where(x => x.chef == null).Where(x => x.dependeencies == null ? true : x.dependeencies.All(dependency => dependency.status == Status.בוצע)));

            }
        }

        if (possibleTasks.Count() == 0)
            throw new BlNoTasksToCompleteException($"אין משימות אפשריות לביצוע עבור השף:{chef.Id}");
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
        BO.Task1? botask = Read(item.Id); //בדיקה שהמשימה קיימת}

        if (item.Copmlexity == BO.ChefExperience.ללא_סינון || item.Copmlexity == null)
            throw new BlChefLevelNoEnteredException("יש להזין רמת קושי");
        if (item.Alias == "")
            throw new BlEmptyStringException("מחרוזת ריקה");
        if (item.RequiredEffortTime == null)
            throw new BlProblemAboutRequiredEffortTimeException("לא ניתן למחוק משך זמן משימה , יש למלא ערך מתאים");

        if (ReadEndProject() == null)
        {
            if (item.chef != null)
                throw new BlInappropriateStepException("לא ניתן להקצות שף למשימה לפני קביעת תאריך סיום הפרויקט");
            if (item.StartDate != null)
                throw new BlInappropriateStepException("לא ניתן לעדכן תאריך התחלה בפועל לפני קביעת תאריך סיום הפרויקט");
            if (item.CompleteDate != null)
                throw new BlInappropriateStepException("לא ניתן לעדכן תאריך סיום בפועל לפני קביעת תאריך סיום הפרויקט");

            if (item.RequiredEffortTime != botask.RequiredEffortTime)
            {
                IEnumerable<DO.Dependeency?>? listDependeenciseOfTask = _dal.Dependeency.ReadAll(X => X.DependsOnTask == item.Id);
                foreach (var depend in listDependeenciseOfTask!)
                {
                    if (_dal.Task1.Read(depend!.DependentTask)!.ScheduledDate != null && _dal.Task1.Read(depend!.DependentTask)!.ScheduledDate < item.ForecastDate)
                        throw new BlProblemAboutRequiredEffortTimeException("לא ניתן משך זמן מתוכנן כך שהמשימה תוכל להיגמר אחרי התאריך המתוכנן של המשימה הבאה אחריה");
                }
            }
            if (item.ScheduledDate == null && botask.ScheduledDate != null)
                throw new BlScheduledStartDateNoUpdatedException("לא ניתן למחוק תאריך תחילה מתוכנן, יש להזין תאריך מתאים");

            if (item.ScheduledDate != null && item.ScheduledDate != botask.ScheduledDate)
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

            else
                item.ScheduledDate = botask.ScheduledDate;

            if (item.dependeencies?.Count > 0)
            {
                foreach (var dependee in item.dependeencies)
                {
                    if (s_bl.Task1.Read(dependee.Id)!.ForecastDate == null)
                        throw new BlCannotaAddDependenciesException("לא ניתן להוסיף לרשימת תלויות משימה בלי תאריך מתוכנן לסיום");
                        if (s_bl.Task1.Read(dependee.Id)!.ForecastDate > item.ScheduledDate)
                        throw new BlEarlyFinishDateFromPreviousTaskException("לא ניתן להכניס משימה שהמשימה הנוכחית תהיה תלויה בה, אם תאריך התחלה המתוכנן של המשימה הנוכחית הוא לפני התאריך סיום המתוכנן של המשימה שרוצים להוסיף");
                }
            }

            if (botask.dependeencies?.Count > 0)
            {
                foreach (var dependee in botask.dependeencies)
                {
                    int idDelete = _dal.Dependeency.Read(X => (X.DependentTask == item.Id && X.DependsOnTask == dependee.Id))!.Id; //מציאת המספר זהות של התלות

                    try
                    {
                        _dal.Dependeency.Delete(idDelete); //נמחוק את כל התלויות שהיו עד עכשיו
                    }

                    catch (DalDoesNotExistException ex)
                    {
                        throw new BlATaskCannotBeDeletedException($"{idDelete}:לא ניתן למחוק את תלות");
                    }
                }
            }

            if (item.dependeencies?.Count > 0)
            {
                foreach (var dependee in item.dependeencies)
                {
                    _dal.Dependeency.Create(new DO.Dependeency(0, item.Id, dependee.Id)); //יצירת כל התלויות החדשות
                }
            }

        }

        else
        {
            if (item.RequiredEffortTime != botask.RequiredEffortTime)
                throw new BlProblemAboutRequiredEffortTimeException("לאחר שלפרויקט יש תאריך סיום, אין לשנות את משך זמן המשימה");

            if (item.ScheduledDate != botask.ScheduledDate)
                throw new BlInappropriateStepException("לא ניתן לשנות תאריך התחלה מתוכנן לאחר קביעת לוח הזמנים");

            if (item.chef == null && botask.chef != null)
                throw new BlTaskAlreadyAssignedException("לא ניתן לבטל שף שכבר קיים במשימה");

            if (item.CompleteDate != null && item.StartDate == null)
                throw new BlWrongDateException("לא ניתן להזין תאריך סיום בפועל לפני תאריך התחלה בפועל");

            if (item.chef == null && item.StartDate != null)
                throw new BlWrongDateException("לא ניתן להזין תאריך התחלה למשימה אם אין לה שף");

            if (item.chef != null)
            {
                if (item.Copmlexity == null || item.Copmlexity == BO.ChefExperience.ללא_סינון) //אם רוצים להקצות שף למשימה יש לבדוק  שיש רמה למשימה
                    throw new BllackingInLevelException("על מנת לשייך שף יש להזין מורכבות את המשימה");

                DO.Chef? _chef = _dal.Chef.Read((int)item.chef.Id!);

                if (_chef == null) //בדיקה שקיים שף מבוקש
                    throw new BlDoesNotExistException($"לא קיים שף עם המספר זהות  {item.Id} ");
                if (botask.chef == null && botask.dependeencies != null)
                    foreach (var task in botask.dependeencies)
                    {
                        var taskBO = Read(task.Id);
                        if (taskBO!.CompleteDate == null)
                            throw new BlNoChangeChefAssignmentException("לא ניתן להקצות שף למשימה בטרם הסתיימו המשימות הקודמות ");
                    }

                if (botask.chef == null || botask.chef.Id != item.chef.Id)
                {
                    DO.Task1? dotask = _dal.Task1.Read(task => task.ChefId == item.chef.Id && task.CompleteDate == null); //חיפוש המשימה שהשף כבר מוקצה לה
                    if (dotask != null && dotask.Id != item.Id && dotask.CompleteDate == null)   // אם השף כבר מוקצה למשימה שאינה זהה למשימה החדשה המעודכנת וגם המשימה הקודמת טרם הושלמה
                        throw new BlNoChangeChefAssignmentException($"השף עם מספר הזהות {item.Id} כבר שויך למשימה שלא הסתיימה");

                    if (botask.chef != null && botask.chef.Id != item.chef.Id)   // לא ניתן להקצות משימה לשף אם המשימה כבר מוקצית לשף אחר
                        throw new BlTaskAlreadyAssignedException($"המשימה עם מספר הזהות {botask.Id} משויכת לשף עם מספר הזהות {botask.chef.Id}");

                    if (item.Copmlexity > (BO.ChefExperience)_chef.Level!) //בדיקה שרמת השף הנדרשת מתאימה לשף המתעדכן
                        throw new BlChefLevelTooLowException("רמת השף נמוכה ממורכבות המשימה");
                }

                else
                {
                    if (item.Copmlexity > (BO.ChefExperience)_chef.Level)
                        throw new BlChefLevelTooLowException("מורכבות המשימה גבוהה מרמת השף");
                }
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
    public bool UpdateScheduledDate(int id, DateTime scheduledDate)
    {
        if (_dal.Task1.ReadEndProject() != null)
            throw new BlInappropriateStepException("לא ניתן לשנות תאריך התחלה מתוכנן למשימה אם יש תאריך סיום לפרויקט");

        DO.Task1? task = _dal.Task1.Read(id);
        if (task == null)
            throw new BlDoesNotExistException($"המשימה עם התעודת מספר הזהות={id} לא קיימת");  //איתור המשימה הנדרשת

        IEnumerable<BO.TaskInList>? listDependeencies = Read(id)!.dependeencies;  //יצירת רשימת תלויות של כל המשימות שהמשימה תלויה בהם  
        if (listDependeencies != null)
        {
            foreach (var taskinlist in listDependeencies)     //מעבר על כל משימה קודמת ובדיקה שתאריך ההתחלה המתוכנן קיים וגם שהתאריך שהתקבל כפרמטר אינו מוקדם מתאריך הסיום המשוער של כל משימה שקודמת לה 
            {
                BO.Task1 task_ = Read(taskinlist.Id)!;
                if (task_.ScheduledDate == null)
                    throw new BlScheduledStartDateNoUpdatedException($"התאריך התחלה של המשימה הקודמת: {taskinlist.Id}, לא מתוכנן");
                if (task_.ForecastDate > scheduledDate)
                    throw new BlEarlyFinishDateFromPreviousTaskException($"לא ניתן לעדכן תאריך מתוכנן להתחלה עבור משימה:{id} מוקדם יותר מתאריך הסיום של משימה קודמת:{task_.Id}");
            }
        }

        IEnumerable<DO.Dependeency?>? listDependeenciseOfTask = _dal.Dependeency.ReadAll(X => X.DependsOnTask == id);

        foreach (var depend in listDependeenciseOfTask!)
        {
            if (_dal.Task1.Read(depend!.DependentTask)!.ScheduledDate != null && _dal.Task1.Read(depend!.DependentTask)!.ScheduledDate < scheduledDate + _dal.Task1.Read(id)!.RequiredEffortTime)
                throw new BlProblemAboutRequiredEffortTimeException("לא ניתן לשנות תאריך מתוכנן להתחלה כך שהמשימה תוכל להסתיים אחרי תאריך התחלה של משימה התלויה בה");
        }

        return true;

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
      .Select(doTask => new BO.TaskInList() { Id = doTask!.Id, Description = doTask.Description, Alias = doTask.Alias, status = this.GetStatus(doTask) });
        if (listTasks.Count() == 0)
            throw new BlNoTasksbyCriterionException("אין משימות שמתאימות לרמת השף");
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
        if (tasks!.Count() == 0)
            throw new BlDoesNotExistException("אין משימות לרמה זאת");

        return (tasks!.Select(doTask => new BO.TaskInList() { Id = doTask!.Id, Description = doTask.Description, Alias = doTask.Alias, status = this.GetStatus(doTask) }));
    }

    /// <summary>
    /// חיפוש משימות שהמשימה הנוכחית לא תלויה בהם כדי להציג על המסך את התלויות שאפשר להוסיף למשימה
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    public IEnumerable<BO.TaskInList> ReadAllNondependenceTask(BO.Task1 task)
    {
        if (task.dependeencies == null)
            return ReadAll();
        IEnumerable<BO.TaskInList> tasksList_ = ReadAll();

        foreach (var task_ in task.dependeencies)
        {
            tasksList_ = tasksList_.Where(x => x.Id != task_.Id);
        }

        tasksList_ = tasksList_.Where(x => x.Id != task.Id);
        return tasksList_;
    }
    /// <summary>
    /// Returning all tasks that have already been completed.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="BlDoesNotExistException"></exception>
    public IEnumerable<BO.TaskInList> ReadAllCompleted()
    {
        IEnumerable<DO.Task1?>? tasks = _dal.Task1.ReadAll(task => task.CompleteDate != null);
        if (tasks!.Count() == 0)
            throw new BlDoesNotExistException("לא הושלמו משימות");
        return tasks!.Select(doTask => new BO.TaskInList() { Id = doTask!.Id, Description = doTask.Description, Alias = doTask.Alias, status = this.GetStatus((DO.Task1)doTask) });
    }

    /// <summary>
    /// Returning all the tasks that are in care.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="BlDoesNotExistException"></exception>

    public IEnumerable<BO.TaskInList> ReadAllTasksInCare()
    {
        IEnumerable<DO.Task1?>? tasks = _dal.Task1.ReadAll(task => task.StartDate != null && task.CompleteDate == null);
        if (tasks!.Count() == 0)
            throw new BlDoesNotExistException("אין משימות המטופלות כעת על ידי שף");
        return tasks!.Select(doTask => new BO.TaskInList() { Id = doTask!.Id, Description = doTask.Description, Alias = doTask.Alias, status = this.GetStatus(doTask) });
    }

    /// <summary>
    /// Returning all tasks for which no chef is assigned.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="BlDoesNotExistException"></exception>

    public IEnumerable<BO.TaskInList> ReadAllNoChefWasAssigned()
    {
        IEnumerable<DO.Task1?>? tasks = _dal.Task1.ReadAll(task => task.ChefId == 0);
        if (tasks!.Count() == 0)
            throw new BlDoesNotExistException("כל המשימות מוקצות לשפים");
        return tasks!.Select(doTask => new BO.TaskInList() { Id = doTask!.Id, Description = doTask.Description, Alias = doTask.Alias, status = this.GetStatus(doTask) });
    }

    /// <summary>
    /// Returning all tasks that do not have a scheduled start date.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="BlDoesNotExistException"></exception>
    public IEnumerable<BO.TaskInList> ReadAllNoScheduledDate()
    {
        IEnumerable<DO.Task1?>? tasks = _dal.Task1.ReadAll(task => task.ScheduledDate == null);
        if (tasks!.Count() == 0)
            throw new BlDoesNotExistException("לכל המשימות יש תאריך התחלה מתוזמן");
        return tasks!.Select(doTask => new BO.TaskInList() { Id = doTask!.Id, Description = doTask.Description, Alias = doTask.Alias, status = this.GetStatus(doTask) });
    }



    public IEnumerable<BO.TaskInList> ReadAllPerStatus(BO.Status status_)
    {
        IEnumerable<BO.TaskInList>? tasks = ReadAll().Where(task => task.status == status_);
        if (tasks!.Count() == 0)
            throw new BlDoesNotExistException($"לא קיימות משימות בסטטוס: {status_}");
        return tasks;
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
            results.Add(new BO.TaskInList() { Id = task.Id, Alias = task.Alias, Description = task.Description, status = this.GetStatus(task) });
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
            status = this.GetStatus(doTask),
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

    public void UpdateClockProject(TimeSpan timeProject)
    {
        DateTime date = _dal.Task1.ReadClockProject();
        date += timeProject;
        _dal.Task1.UpdateClockProject(date);
    }
    public DateTime ReadClockProject()
    {
        return _dal.Task1.ReadClockProject();
    }

    public void UpdateStartDate(BO.Task1 item)
    {
        DO.Task1? task1 = _dal.Task1.Read(item.Id);

        if (task1 == null)
        {
            throw new BlDoesNotExistException($"לא קיימת משימה עם מספר זהות {item.Id}");
        }

        IEnumerable<BO.TaskInList>? listDependeencies = Read(item.Id)!.dependeencies;  //יצירת רשימת תלויות של כל המשימות שהמשימה תלויה בהם  
        if (listDependeencies != null)
        {
            foreach (var taskinlist in listDependeencies) //בדיקה שכל המשימות הקודומת הסתיימו
            {
                BO.Task1 task_ = Read(taskinlist.Id)!;
                if (task_.CompleteDate == null)
                    throw new BlUnableToStartTaskException($"לא ניתן להתחיל משימה לפני שהמשימות הקודמות הושלמו");
            }
        }

        _dal.Task1.Update(new DO.Task1(item.Id, item.Alias, item.Description, item.CreatedAtDate, item.ScheduledDate,
                           item.RequiredEffortTime, item.DeadlineDate, item.chef!.Id,
                           item.StartDate, item.CompleteDate, (DO.ChefExperience)item.Copmlexity!, item.Dellverables,
                           item.Remarks, null));

    }

    /// <summary>
    /// עדכון תאריך סיום
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="BlDoesNotExistException"></exception>
    /// <exception cref="BlAlreadyExistsException"></exception>
    public void UpdateFinalDate(BO.Task1 item)
    {
        DO.Task1? task1 = _dal.Task1.Read(item.Id);

        if (task1 == null)
        {
            throw new BlDoesNotExistException($"לא קיימת משימה עם מספר זהות {item.Id}");
        }

        if (task1.CompleteDate != null)
        {
            throw new BlAlreadyExistsException("תאריך הסיום בפועל כבר עודכן");
        }

        _dal.Task1.Update(new DO.Task1(item.Id, item.Alias, item.Description, item.CreatedAtDate, item.ScheduledDate,
                           item.RequiredEffortTime, item.DeadlineDate, item.chef!.Id,
                           item.StartDate, item.CompleteDate, (DO.ChefExperience)item.Copmlexity!, item.Dellverables,
                           item.Remarks, null));

    }
}