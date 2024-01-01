namespace DO;
public record Engineer
(
    int It,
    string? Email = null,
    double? Cost = null,
    string? Name = null,
    DO.EngineerExperience Level
 );
