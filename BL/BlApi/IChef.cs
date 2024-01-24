namespace BlApi;
/// <summary>
/// Interface for the logical entity "Chef"
/// </summary>

public interface IChef
{
    public IEnumerable<BO.Chef> ReadAll(Func<DO.Chef, bool>? filter = null);
    public BO.Chef? Read(int id);
    public int Create(BO.Chef item);
    public void Delete(int id);
    public void Update(BO.Chef item);
}
