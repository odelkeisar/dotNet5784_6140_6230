namespace BlApi;
/// <summary>
/// Interface for the logical entity "Chef"
/// </summary>

public interface IChef
{
    public int Create(BO.Chef item);

    public IEnumerable<BO.Chef>? ReadAll();
    public IEnumerable<BO.Chef>? ReadAllPerLevel(BO.ChefExperience level);
    public IEnumerable<BO.Chef>? ReadAllNotAssigned();
    public BO.Chef? Read(int id);
    public void RecoveryChef(BO.Chef chef);

    public void Delete(int id);
    public void Update(BO.Chef item);
    public IEnumerable<BO.Chef> ReadAllDeleted();

}
