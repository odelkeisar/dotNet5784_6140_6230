using BlApi;
using BO;
using DO;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Security.Cryptography;
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
    public int Create(BO.Task1 item)
    {
        if (item.Id < 0)
            throw new BlWrongNegativeIdException("Task with negative ID");
        if (item.Alias == "")
            throw new BlEmptyStringException("The string is empty");

        bool flag = true;

        if (item.Milestone == null) //אם זה שווה לנל אין לנו אבן דרך
            flag = false;



        DO.Task1 doTask = new DO.Task1
       (0, item.Alias, item.Description, item.CreatedAtDate, item.ScheduledDate, item.RequiredEffortTime,
       item.DeadlineDate, item.chef!.Id, item.StartDate, item.CompleteDate, (DO.ChefExperience)item.Copmlexity!,
       item.Dellverables, item.Remarks, flag);

        try
        {
            int idTask1 = _dal.Task1.Create(doTask);
            return idTask1;
        }

        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Student with ID={item.Id} already exists", ex);
        }

    }

    public void Delete(int id)
    {


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
            throw new BO.BlDoesNotExistException($"Student with ID={id} does Not exist");

        BO.Task1? chef2 = new BO.Task1()
        {
            Id = id,
            Alias = doTask.Alias,
            Description = doTask.Description,
            CreatedAtDate = doTask.CreatedAtDate,
            ScheduledDate = doTask.ScheduledDate,
            RequiredEffortTime = doTask.RequiredEffortTime,
            DeadlineDate = doTask.DeadlineDate,
            //פה צריך להיות תעודת זהות שף
            StartDate = doTask.StartDate,
            CompleteDate = doTask.CompleteDate,
            Copmlexity = (BO.ChefExperience)doTask.Copmlexity!,
            Dellverables = doTask.Dellverables,
            Remarks = doTask.Remarks
        };

    }

    public IEnumerable<BO.Task1> ReadAllLevel(BO.Chef chef)
    {
        return _dal.Task1.ReadAll()!
      .Where(doTask => doTask!.Copmlexity == (DO.ChefExperience)chef.Level!)
      .Select(doTask => new BO.Task1()
      {
          Id = doTask!.Id,
          Alias = doTask.Alias,
          Description = doTask.Description,
          CreatedAtDate = doTask.CreatedAtDate,
          ScheduledDate = doTask.ScheduledDate,
          RequiredEffortTime = doTask.RequiredEffortTime,
          DeadlineDate = doTask.DeadlineDate,
          status = GetStatus(doTask),
          dependeencies=GetTaskInList(doTask.Id),
          StartDate = doTask.StartDate,
          
          
          CompleteDate=doTask.CompleteDate,
          
          Dellverables=doTask.Dellverables,
          Remarks=doTask.Remarks,
          
         

      });
    }


   
    
    
    public DateTime? ForecastDate
    {
        get { return ScheduledDate < StartDate ? StartDate + RequiredEffortTime : ScheduledDate + RequiredEffortTime; }
    }
    public DateTime? DeadlineDate { get; set; }//תאריך סיום
 
    public TimeSpan? RequiredEffortTime { get; set; } //משך זמן המשימה
    public ChefInTask? chef { get; set; }
    public ChefExperience? Copmlexity { get; set; } //רמת קושי


    public void Update(BO.Task1 item)
    {
        throw new NotImplementedException();
    }

    public void UpdateStartTime(int id, DateTime dateTime)
    {
        throw new NotImplementedException();
    }

    public Status GetStatus(DO.Task1 task)
    {
        if (task.CompleteDate != null)
            return Status.Done;

        if (task.ChefId != null)
            return Status.OnTrack;

        if (task.ScheduledDate != null)
            return Status.Scheduled;

        return Status.Unscheduled;
    }

    public List<TaskInList>? GetTaskInList(int id)
    {
        IEnumerable<DO.Dependeency> ?listDependencies = _dal.Dependeency.ReadAll(X => X.DependentTask == id)!;
        var results = listDependencies.Select(dependency => _dal.Task1.Read(dependency.DependsOnTask)).
            Select(X=>new TaskInList() {Id=X.Id, Alias=X.Alias, Description=X.Description, status= GetStatus(X)});
        return results.ToList();
    }
}
