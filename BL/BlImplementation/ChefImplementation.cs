using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Threading.Tasks;
namespace BlImplementation;
internal class ChefImplementation : IChef
{
    private DalApi.IDal _dal = Factory.Get;

    /// <summary>
    /// Create object of BO.Chef
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlWrongNegativeIdException"></exception>
    /// <exception cref="BO.BlEmptyStringException"></exception>
    /// <exception cref="BO.BlNegativeHourlyWageException"></exception>
    /// <exception cref="BO.BlWrongEmailException"></exception>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    public int Create(BO.Chef item)
    {
        if (item.Id < 100000000 || item.Id >999999999)
            throw new BO.BlWrongNegativeIdException($"מספר זהות לא חוקי");
        if (item.Name == " ")
            throw new BO.BlEmptyStringException($"נא להכניס מספר זהות");
        if (item.Cost < 0)
            throw new BO.BlNegativeHourlyWageException($"משכורת לפי שעה צריך להיות חיובית");
        if (item.Email == " ")
            throw new BO.BlEmptyStringException($"חייב להכניס כתובת דואר אלקטרוני");
        if (!item.Email!.Contains("@"))
            throw new BO.BlWrongEmailException($"כתובת מייל שגויה");
        if (item.Level == null || item.Level == ChefExperience.ללא_סינון)
            throw new BlChefLevelNoEnteredException($"נא להכניס רמת שף");
        if (item.task != null)
            throw new BlUnablToAssociateException("בזמן יצירת השף לא ניתן להוסיף לו משימה");
        DO.Chef chef = new DO.Chef(item.Id, item.deleted, item.Email, item.Cost, item.Name, (DO.ChefExperience)item.Level!);

        try
        {
            int idChef = _dal.Chef.Create(chef);
            return idChef;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"כבר קיים שף עם מספר זהות זה", ex);
        }
    }

    /// <summary>
    /// Delete object of Chef
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BlChefOnTaskException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>

    public void Delete(int id)
    {
        DO.Task1? task = _dal.Task1.Read(t => t.ChefId == id);
        if (task != null)
            throw new BlChefOnTaskException($"השף עם מספר הזהות {id} שויך למשימה ולכן לא ניתן למחוק אותו");
        try
        {
            _dal.Chef.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"לא קיים שף עם מספר מזהה{id} ", ex);
        }
    }
    
    /// <summary>
    ///  שיחזור שף מהארכיון
    /// </summary>
    /// <param name="chef"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
     public void RecoveryChef(BO.Chef chef)
    {
        try
        {
            DO.Chef chef1=new DO.Chef() { deleted = false, Id =chef.Id, Cost=chef.Cost, Email=chef.Email,Level= chef.Level!=null?(DO.ChefExperience)chef.Level:DO.ChefExperience.כולם, Name=chef.Name};
            _dal.Chef.Recovery(chef1);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Chef with ID={chef.Id} does not exists", ex);
        }
    }

    /// <summary>
    /// Search and read object of BO.Chef
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public BO.Chef? Read(int id)
    {
        DO.Chef? chef1 = _dal.Chef.Read(id);
        if (chef1 == null)
            throw new BO.BlDoesNotExistException($"לא קיים שף עם מספר מזהה{id}");
        DO.Task1? task = new DO.Task1();    
        IEnumerable< DO.Task1?>?listTask= _dal.Task1.ReadAll(t => t.ChefId == id && t.CompleteDate == null);
        if (listTask != null)
        {
            task= listTask.FirstOrDefault();
            foreach (var task_ in listTask) //בדיקה גילוי המשימה המוקדמת ביותר שהשף משוייך אליה בטרם הסתיימה
            {
                if (task_!.ScheduledDate < task!.ScheduledDate)
                    task = task_;
            }
        }

        TaskInChef? taskInChef;

        if (task == null)
            taskInChef = null;
        else
            taskInChef = new TaskInChef() { Id = task.Id, Alias = task.Alias };

        BO.Chef? chef2 = new BO.Chef() { Id = chef1.Id, deleted = chef1.deleted, Email = chef1.Email, Cost = chef1.Cost, Name = chef1.Name, Level = (BO.ChefExperience)chef1.Level!, task = taskInChef };
        return chef2;
    }

    /// <summary>
    /// Read all the objects that in list
    /// </summary>
    /// <returns></returns>
    public IEnumerable<BO.Chef> ReadAll()
    {
        return (from DO.Chef chef in _dal.Chef.ReadAll()!
                let chef_ = Read(chef.Id)
                select new BO.Chef
                {
                    Id = chef_.Id,
                    deleted = chef_.deleted,
                    Email = chef_.Email,
                    Cost = chef_.Cost,
                    Name = chef_.Name,
                    Level = (BO.ChefExperience)chef_.Level!,
                    task = chef_.task != null ? new TaskInChef() { Id = chef_.task.Id, Alias = chef_.task.Alias } : null
                }
        );
    }

    public IEnumerable<BO.Chef> ReadAllDeleted ()
    {
        var listChefs= (from DO.Chef chef in _dal.Chef.ReadAll_deleted()!
              
                select new BO.Chef
                {
                    Id = chef.Id,
                    deleted = chef.deleted,
                    Email = chef.Email,
                    Cost = chef.Cost,
                    Name = chef.Name,
                    Level = (BO.ChefExperience)chef.Level!,
                    task = null
                }
        );
        if( listChefs.Count()==0 )
            throw new BlNoChefsDeletedException("אין שפים בארכיון");
        return listChefs;   
    }

    /// <summary>
    /// Read object according level of chef.
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    /// <exception cref="BlNoChefsAccordingLevelException"></exception>

    public IEnumerable<BO.Chef> ReadAllPerLevel(BO.ChefExperience level)
    {
        IEnumerable<DO.Chef>? listChef = _dal.Chef.ReadAll(x => (BO.ChefExperience)x.Level! == level)!;
        if (listChef == null)
            throw new BlNoChefsAccordingLevelException($"אין שפים ברמת {level}");

        return (from DO.Chef chef in listChef!
                let chef_ = Read(chef.Id)
                select new BO.Chef
                {
                    Id = chef_.Id,
                    deleted = chef_.deleted,
                    Email = chef_.Email,
                    Cost = chef_.Cost,
                    Name = chef_.Name,
                    Level = (BO.ChefExperience)chef_.Level!,
                    task = chef_.task != null ? new TaskInChef() { Id = chef_.task.Id, Alias = chef_.task.Alias } : null
                });
    }

    /// <summary>
    /// Searching for all chefs not assigned to a task.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="BlNoUnassignedChefsException"></exception>
    public IEnumerable<BO.Chef> ReadAllNotAssigned()
    {
        var ChefList = _dal.Chef.ReadAll()!.Select(chef => new BO.Chef
        {
            Id = chef!.Id,
            deleted = chef.deleted,
            Email = chef.Email,
            Cost = chef.Cost,
            Name = chef.Name,
            Level = (BO.ChefExperience)chef.Level!,
            task = Read(chef.Id)!.task

        }).Where(chef => chef.task == null);

        if (ChefList == null)
            throw new BlNoUnassignedChefsException("אין שפים שלא מוקצים למשימה");
        return ChefList;
    }

    /// <summary>
    /// Update the object of chef.
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="BlDoesNotExistException"></exception>
    /// <exception cref="BO.BlEmptyStringException"></exception>
    /// <exception cref="BO.BlNegativeHourlyWageException"></exception>
    /// <exception cref="BO.BlWrongEmailException"></exception>
    /// <exception cref="BlChefLevelTooLowException"></exception>
    /// <exception cref="BlNoChangeChefAssignmentException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BlTaskAlreadyAssignedException"></exception>
    public void Update(BO.Chef item)
    {
        BO.Chef? chef = Read(item.Id);
        if (chef == null)
            throw new BlDoesNotExistException($"לא קיים שף עם מספר זהות {item.Id} ");
        if (item.Name == "")
            throw new BO.BlEmptyStringException($"חובה להכניס שם");
        if (item.Cost < 0)
            throw new BO.BlNegativeHourlyWageException($"נא להכניס משכורת לפי שעה חיובית");
        if (item.Email == "")
            throw new BO.BlEmptyStringException($"נא להכניס כתובת דואר אלקטרוני");
        if (!item.Email!.Contains("@"))
            throw new BO.BlWrongEmailException($"כתובת דואר אלקטרוני שגויה");
        if (chef.Level > (BO.ChefExperience)item.Level!)
            throw new BlChefLevelTooLowException($"לא ניתן לעדכן רמת שף נמוכה מהנוכחית");
        
       
        if (item.task != null)
        {
            DO.Task1? task1 = _dal.Task1.Read(task => task.ChefId == item.Id&& task.CompleteDate==null); //חיפוש המשימה שהשף כבר מוקצה לה
            if (task1 != null && item.task.Id != task1.Id && task1.CompleteDate == null)   // אם השף כבר מוקצה למשימה שאינה זהה למשימה החדשה המעודכנת וגם המשימה הקודמת טרם הושלמה
                throw new BlNoChangeChefAssignmentException($"השף עדיין לא סיים את משימתו הקודמת");

            DO.Task1? task2 = _dal.Task1.Read(t => t.Id == item.task.Id); //חיפוש המשימה אותה השף מעוניין להקצות לעצמו
            if (task2 == null) //אם אין משימה כזו
                throw new BlDoesNotExistException($"לא קיימת משימה עם מספר זהות {item.task.Id} "); //אם המשימה לא קיימת

            if (task2.ChefId != 0 && task2.ChefId != item.Id)    //אם המשימה כבר מוקצית לשף אחר
                throw new BlTaskAlreadyAssignedException($"המשימה עם מספר הזהות {item.task.Id} הוקצתה כבר לשף עם מספר הזהות {task2.ChefId}");


            if (_dal.Task1.ReadEndProject() == null) //אם לפרויקט אין תאריך סיום מתוכנן
                throw new BlScheduledStartDateNoUpdatedException("לא ניתן להקצות שף למשימה כאשר אין תאריך סיום מתוכנן לפרויקט");

            if (task2.Copmlexity == null)
                throw new BllackingInLevelException("על מנת לשייך שף יש להזין את מורכבות המשימה");

            if ((DO.ChefExperience)item.Level < task2.Copmlexity)
                throw new BlChefLevelTooLowException($"רמת המהנדס נמוכה ממורכבות המשימה");

            try { _dal.Task1.Update(task2 with { ChefId = item.Id }); }//עדכון הקצאת השף למשימה
            catch (DO.DalDoesNotExistException ex) { throw new BO.BlDoesNotExistException($"השף עם המספר זהות{item.Id} לא קיים", ex); }
        }

        _dal.Chef.Update(new DO.Chef(item.Id, item.deleted, item.Email, item.Cost, item.Name, (DO.ChefExperience)item.Level!)); //עדכון פרטי שף.
    }
}