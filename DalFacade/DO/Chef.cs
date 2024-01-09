namespace DO;
/// <summary>
/// Engineer
/// </summary>
/// <param name="Id">Unique ID number</param>
/// <param name="Email">Email</param>
/// <param name="Cost">cost per hour</param>
/// <param name="Name">Engineer's name (full name)</param>
/// <param name="Level">Engineer level</param>
public record Chef
(
    int Id,
    string? Email = null,
    double? Cost = null,
    string? Name = null,
    ChefExperience? Level = null
 )
{
    public Chef() : this(0){ }
}