namespace DO;
public record Engineer
(
    int It,
    DO.EngineerExperience Level,
    string? Email = null,
    double? Cost = null,
    string? Name = null
 );
