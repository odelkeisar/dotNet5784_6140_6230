using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        if (item.Id < 0)
            throw new BO.BlWrongNegativeIdException($"The ID={item.Id} is negative");
        if (item.Name == " ")
            throw new BO.BlEmptyStringException($"The chef's name field with the ID:{item.Id} is empty");
        if (item.Cost < 0)
            throw new BO.BlNegativeHourlyWageException($"The cost of ID={item.Id} is negative");
        if (item.Email == " ")
            throw new BO.BlEmptyStringException($"The chef's mail field with the ID:{item.Id} is empty");
        if (!item.Email!.Contains("@"))
            throw new BO.BlWrongEmailException($"The mail of ID={item.Id} is wrong");

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

        DO.Task1? task = _dal.Task1.Read(t => t.ChefId == id);

        TaskInChef? taskInChef = null;

        if (task != null)
            taskInChef = new TaskInChef() { Id = task.Id, Alias = task.Alias };

        BO.Chef? chef2 = new BO.Chef() { Id = chef1.Id, deleted = chef1.deleted, Email = chef1.Email, Cost = chef1.Cost, Name = chef1.Name, Level = (BO.ChefExperience)chef1.Level!, task = taskInChef };
        return chef2;
    }

    /// <summary>
    /// Read all the objects that in list
    /// </summary>
    /// <returns></returns>
    public IEnumerable<BO.Chef>? ReadAll()
    {
        return (from DO.Chef chef in _dal.Chef.ReadAll()!
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
                }
        );
    }

    /// <summary>
    /// Read object according level of chef.
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    /// <exception cref="BlNoChefsAccordingLevelException"></exception>

    public IEnumerable<BO.Chef>? ReadAllPerLevel(BO.ChefExperience level)
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
    public IEnumerable<BO.Chef>? ReadAllNotAssigned()
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
        if (item.Name == " ")
            throw new BO.BlEmptyStringException($"The chef's name field with the ID:{item.Id} is empty");
        if (item.Cost < 0)
            throw new BO.BlNegativeHourlyWageException($"The cost of ID={item.Id} is negative");
        if (item.Email == " ")
            throw new BO.BlEmptyStringException($"The chef's mail field with the ID:{item.Id} is empty");
        if (!item.Email!.Contains("@"))
            throw new BO.BlWrongEmailException($"The mail of ID={item.Id} is wrong");
        if (chef.Level > (BO.ChefExperience)item.Level!)
            throw new BlChefLevelTooLowException($"For the chef with the ID:{item.Id}, it is not possible to update a chef level lower than the existing one");

        DO.Task1? task_ = _dal.Task1.Read(task => task.ChefId == chef.Id);

        if (task_ != null && task_.CompleteDate == null)
            throw new BlNoChangeChefAssignmentException($"The chef with{item.Id} is already assigned to an unfinished task");

        if (item.task != null)
        {

            DO.Task1? task1 = _dal.Task1.Read(t => t.Id == item.task.Id);
            if (task1 != null && task1.ChefId == 0)
            {
                DO.Task1 task2 = task1 with { ChefId = item.Id };
                try { _dal.Task1.Update(task1); }
                catch (DO.DalDoesNotExistException ex) { throw new BO.BlDoesNotExistException($"Chef with ID={item.Id} does not exists", ex); }
            }
            else
            {
                if (task1 == null)
                    throw new BlDoesNotExistException($"Task with ID={item.task.Id} does not exists");
                if (task1.ChefId != 0)
                    throw new BlTaskAlreadyAssignedException($"The task with the ID{item.task.Id} is already assigned to the chef with the ID {task1.ChefId}");
            }
        }
      
        _dal.Chef.Update(new DO.Chef(item.Id, item.deleted, item.Email, item.Cost, item.Name, (DO.ChefExperience)item.Level!));
    }
}



