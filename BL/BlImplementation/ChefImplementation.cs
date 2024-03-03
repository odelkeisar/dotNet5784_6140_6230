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
            throw new BO.BlWrongNegativeIdException($"The ID={item.Id} Invalid");
        if (item.Name == " ")
            throw new BO.BlEmptyStringException($"The chef's name field with the ID:{item.Id} is empty");
        if (item.Cost < 0)
            throw new BO.BlNegativeHourlyWageException($"The cost of ID={item.Id} is negative");
        if (item.Email == " ")
            throw new BO.BlEmptyStringException($"The chef's mail field with the ID:{item.Id} is empty");
        if (!item.Email!.Contains("@"))
            throw new BO.BlWrongEmailException($"The mail of ID={item.Id} is wrong");
        if (item.Level == null || item.Level == ChefExperience.None)
            throw new BlChefLevelNoEnteredException($"Chef ID:{item.Id} lacks a field of level of experience");
        if (item.task != null)
            throw new BlUnablToAssociateException("A task cannot be assigned to a chef while he is being added to the list");
        DO.Chef chef = new DO.Chef(item.Id, item.deleted, item.Email, item.Cost, item.Name, (DO.ChefExperience)item.Level!);

        try
        {
            int idChef = _dal.Chef.Create(chef);
            return idChef;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Chef with ID={item.Id} already exists", ex);
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
            throw new BlChefOnTaskException($"The chef with ID {id} has already finished performing a task or is actively performing a task");
        try
        {
            _dal.Chef.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Chef with ID={id} does not exists", ex);
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
            throw new BO.BlDoesNotExistException($"Chef with ID={id} does not exists");
        DO.Task1? task = new DO.Task1();    
        IEnumerable< DO.Task1?>?listTask= _dal.Task1.ReadAll(t => t.ChefId == id && t.CompleteDate == null);
        if (listTask != null)
        {
            task= listTask.FirstOrDefault();
            foreach (var task_ in listTask)
            {
                if (task_!.ScheduledDate < task!.ScheduledDate)
                    task = task_;
            }
        }

        TaskInChef? taskInChef;

        if (task == null)
            taskInChef = null;
        else
            taskInChef = new() { Id = task.Id, Alias = task.Alias };

        if (task != null)
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
            throw new BlNoChefsAccordingLevelException($"There are no chefs at the level of:{level}");

        return (from DO.Chef chef in listChef!
                let task = _dal.Task1.Read(t => t.ChefId == chef.Id)
                select new BO.Chef
                {
                    Id = chef.Id,
                    deleted = chef.deleted,
                    Email = chef.Email,
                    Cost = chef.Cost,
                    Name = chef.Name,
                    Level = (BO.ChefExperience)chef.Level!,
                    task = task != null ? new TaskInChef() { Id = task.Id, Alias = task.Alias } : null
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
            throw new BlNoUnassignedChefsException("There are no chefs that are not assigned to a task");
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
            throw new BlDoesNotExistException($"Chef with ID={item.Id} does not exists");
        if (item.Name == "")
            throw new BO.BlEmptyStringException($"The chef's name field with the ID:{item.Id} is empty");
        if (item.Cost < 0)
            throw new BO.BlNegativeHourlyWageException($"The cost of ID={item.Id} is negative");
        if (item.Email == "")
            throw new BO.BlEmptyStringException($"The chef's mail field with the ID:{item.Id} is empty");
        if (!item.Email!.Contains("@"))
            throw new BO.BlWrongEmailException($"The mail of ID={item.Id} is wrong");
        if (chef.Level > (BO.ChefExperience)item.Level!)
            throw new BlChefLevelTooLowException($"For the chef with the ID:{item.Id}, it is not possible to update a chef level lower than the existing one");
        
       
        if (item.task != null)
        {
            DO.Task1? task1 = _dal.Task1.Read(task => task.ChefId == item.Id&& task.CompleteDate==null); //חיפוש המשימה שהשף כבר מוקצה לה
            if (task1 != null && item.task.Id != task1.Id && task1.CompleteDate == null)   // אם השף כבר מוקצה למשימה שאינה זהה למשימה החדשה המעודכנת וגם המשימה הקודמת טרם הושלמה
                throw new BlNoChangeChefAssignmentException($"The chef with ID: {item.Id} is already assigned to an unfinished task");

            DO.Task1? task2 = _dal.Task1.Read(t => t.Id == item.task.Id); //חיפוש המשימה אותה השף מעוניין להקצות לעצמו
            if (task2 == null) //אם אין משימה כזו
                throw new BlDoesNotExistException($"Task with ID={item.task.Id} does not exists"); //אם המשימה לא קיימת

            if (task2.ChefId != 0 && task2.ChefId != item.Id)    //אם המשימה כבר מוקצית לשף אחר
                throw new BlTaskAlreadyAssignedException($"The task with the ID{item.task.Id} is already assigned to the chef with the ID {task2.ChefId}");


            if (_dal.Task1.ReadEndProject() == null) //אם לפרויקט אין תאריך סיום מתוכנן
                throw new BlScheduledStartDateNoUpdatedException("A chef cannot be assigned to a task when there is no scheduled end date for the project");

            if (task2.Copmlexity == null)
                throw new BllackingInLevelException("In order to associate a chef, complexity must be entered");

            if ((DO.ChefExperience)item.Level < task2.Copmlexity)
                throw new BlChefLevelTooLowException($"The level of the chef ID:{item.Id} is lower than the complexity of the task");

            try { _dal.Task1.Update(task2 with { ChefId = item.Id }); }//עדכון הקצאת השף למשימה
            catch (DO.DalDoesNotExistException ex) { throw new BO.BlDoesNotExistException($"Chef with ID={item.Id} does not exists", ex); }
        }

        _dal.Chef.Update(new DO.Chef(item.Id, item.deleted, item.Email, item.Cost, item.Name, (DO.ChefExperience)item.Level!)); //עדכון פרטי שף.
    }
}



