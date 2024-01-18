namespace DalApi;
using DO;
public interface IDependency : ICrud<Dependency>
{
    // returns true if the dependency already exists
    //bool DependencyExists(int dependentTask, int dependentOnTask);

}
