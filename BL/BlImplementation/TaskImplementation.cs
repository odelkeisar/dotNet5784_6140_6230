using BlApi;

namespace BlImplementation;
internal class TaskImplementation : ITask1
{
    public int Create(BO.Task1 item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public BO.Task1? Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.Task1> ReadAll(Func<DO.Task1, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Task1 item)
    {
        throw new NotImplementedException();
    }

    public void UpdateStartTime(int id, DateTime dateTime)
    {
        throw new NotImplementedException();
    }
}
