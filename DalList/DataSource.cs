using DO;

namespace Dal;

internal static class DataSource
{

    internal static class Config
    {
        internal const int startDependencyId = 1000;

        private static int nextDependencyId = startDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }


        internal const int startTaskId = 1000;

        private static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++; }

        internal static DateTime? StartDate = null;

        internal static DateTime? EndDate = null;
    }
    public static Dependency? FirstOrDefault(int id) { return DataSource.Dependencies.FirstOrDefault(item => item.Id == id); }


    internal static List<DO.Dependency> Dependencies { get; } = new();
    internal static List<DO.Task> Tasks { get; } = new();
    internal static List<DO.Engineer> Engineers { get; } = new();
}
