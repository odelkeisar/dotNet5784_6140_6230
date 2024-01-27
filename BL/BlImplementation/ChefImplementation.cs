using BlApi;

namespace BlImplementation;
internal class ChefImplementation : IChef
{
    private DalApi.IDal _dal = Factory.Get;

    public int Create(BO.Chef item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public BO.Chef? Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.Chef> ReadAll(Func<DO.Chef, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Chef item)
    {
        throw new NotImplementedException();
    }
}
