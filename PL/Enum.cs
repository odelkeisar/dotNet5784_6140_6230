namespace PL;


internal class ChefLevel : IEnumerable
{
    static readonly IEnumerable<BO.ChefExperience> s_enums =
(Enum.GetValues(typeof(BO.ChefExperience)) as IEnumerable<BO.ChefExperience>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

