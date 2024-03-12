using DO;

namespace BO;

/// <summary>
/// Chef
/// </summary>
/// <param name="Id">Unique ID number</param>
/// <param name="Email">Email</param>
/// <param name="Cost">cost per hour</param>
/// <param name="Name">Engineer's name (full name)</param>
/// <param name="Level">Engineer level</param>
public class Chef
{
    public int Id { get; init; }
    public bool deleted { get; set; }
    public string? Email { get; set; }
    public double? Cost { get; set; }
    public string? Name { get; set; }
    public ChefExperience? Level { get; set; }
    public TaskInChef? task { get; set; }
    public override string ToString() => ToString_();
    public string ToString_()
    {
        return ($@"
{Id} =מספר זהות
שם= {Name}  
{Email} =דואר אלקטרוני   
רמת שף= {Level}
{Cost} =מחיר לשעה
{task} =משימה
");
    }

}
