namespace BlApi;
/// <summary>
/// Interface for the logical entity "Task1"
/// </summary>

public interface ITask1
{
    public IEnumerable<BO.Task1> ReadAll(Func<DO.Task1, bool>? filter = null);
    public BO.Task1? Read(int id);
    public int Create(BO.Task1 item);
    public void Update(BO.Task1 item);
    public void Delete(int id);
    public void UpdateScheduledDate(int id, DateTime scheduledDate);
}
