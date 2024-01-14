namespace DalApi;

using DalFacade;
using DO;
public interface IDependency : ICrud<Dependency>
{
    bool DependencyExists(int dependentTask, int dependentOnTask);
}
