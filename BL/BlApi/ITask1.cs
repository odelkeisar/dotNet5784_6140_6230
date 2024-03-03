

namespace BlApi;
/// <summary>
/// Interface for the logical entity "Task1"
/// </summary>

public interface ITask1
{
    public DateTime? ReadStartProject();

    public DateTime? ReadEndProject();
    public void CreateStartProject(DateTime? starProject);
    public void CreateEndProject(DateTime? endProject);
    public int Create(BO.Task1 item);
    public void Delete(int id);
    public BO.Task1? Read(int id);
    public IEnumerable<BO.TaskInList> ReadAll();
    public IEnumerable<BO.TaskInList> ReadAllPossibleTasks(BO.Chef chef);

    public IEnumerable<BO.TaskInList> ReadAllPerLevelOfChef(BO.Chef chef);
    public IEnumerable<BO.TaskInList> ReadAllPerStatus(BO.Status status_);
    public IEnumerable<BO.TaskInList> ReadAllNondependenceTask(BO.Task1 task);



    public IEnumerable<BO.TaskInList> ReadAllPerLevel(BO.ChefExperience _level);
    public IEnumerable<BO.TaskInList> ReadAllCompleted();

    public IEnumerable<BO.TaskInList> ReadAllTasksInCare();
    public IEnumerable<BO.TaskInList> ReadAllNoChefWasAssigned();
    public IEnumerable<BO.TaskInList> ReadAllNoScheduledDate();
    public void Update(BO.Task1 item);
    public bool UpdateScheduledDate(int id, DateTime scheduledDate);
    public void UpdateClockProject(TimeSpan timeProject);
    public DateTime ReadClockProject();
    public void UpdateStartDate(BO.Task1 item);
    public void UpdateFinalDate(BO.Task1 item);
}