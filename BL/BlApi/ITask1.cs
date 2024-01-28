namespace BlApi;
/// <summary>
/// Interface for the logical entity "Task1"
/// </summary>

public interface ITask1
{
    public int Create(BO.Task1 item);
    public void Delete(int id);
    public BO.Task1? Read(int id);
    public IEnumerable<BO.Task1>? ReadAll();
    public IEnumerable<BO.Task1>? ReadAllPossibleTasks(BO.Chef chef);
    public IEnumerable<BO.Task1>? ReadAllPerLevel(BO.Chef chef);
    public void Update(BO.Task1 item);
    public void UpdateScheduledDate(int id, DateTime scheduledDate);
}
