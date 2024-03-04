using DO;

namespace DalApi;
/// <summary>
/// call to Icrud with type of Chef
/// </summary>
public interface IChef : ICrud<Chef> 
{
    public IEnumerable<Chef> ReadAll_deleted();
    public void Recovery(Chef item);

}






