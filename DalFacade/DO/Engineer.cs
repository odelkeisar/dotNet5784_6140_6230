namespace DO;
/// <summary>
/// Engineer
/// </summary>
/// <param name="Id">Unique ID number</param>
/// <param name="Email">Email</param>
/// <param name="Cost">cost per hour</param>
/// <param name="Name">Engineer's name (full name)</param>
/// <param name="Level">Engineer level</param>
public record Engineer
(
    int Id,
    string? Email = null,
    double? Cost = null,
    string? Name = null,
    DO.EngineerExperience? Level=null
 );
