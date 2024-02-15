namespace BO;
/// <summary>
/// Id and name of chef
/// </summary>
public class ChefInTask
{
    public int Id { get; init; }
    public string ?Name { get; set; }
    public override string ToString() => ToString_();
    public string ToString_() => $"Name= {Name}, Id= {Id} "; 
}
