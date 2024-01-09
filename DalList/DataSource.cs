namespace Dal;
 internal static class DataSource
{
    internal static List<DO.Task1>? Tasks { get; } = new();
    internal static List<DO.Engineer>? Engineers { get; } = new();
    internal static List<DO.Dependeency>? Dependeencies { get; } = new();

    internal static class Config
    {
        internal const int StartTaskId = 1;
        private static int nextTaskId = StartTaskId;
        internal static int NextTaskId { get => nextTaskId++; }

        internal const int StartDependeencyId = 1;
        private static int nextDependeencyId = StartDependeencyId;
        internal static int NextDependeencyId { get => nextDependeencyId++; }
    }

}
