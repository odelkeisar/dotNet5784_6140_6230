namespace DO;

public record Dependeency
(
    int Id,
    int DependentTask,
    int DependsOnTask
 );
