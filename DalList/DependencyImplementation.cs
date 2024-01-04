namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int id = DataSource.Config.NextDependencyId;
        Dependency copyItem = item with { Id = id };
        DataSource.Dependencies.Add(copyItem);
        return id;
    }

    public void Delete(int id)
    {
        if (Read(id) is null)
            throw new Exception($"Dependency with ID={id} does not exists");
        DataSource.Dependencies.Remove(Read(id)!);
    }

    public Dependency? Read(int id)
    {
        return DataSource.Dependencies.Find(x => x.Id == id);
    }

    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencies);
    }

    public void Update(Dependency item)
    {
        if (Read(item.Id) is null)
            throw new Exception($"Dependency with ID={item.Id} does not exists");
        Delete(item.Id);
        DataSource.Dependencies.Add(item);
    }
}
