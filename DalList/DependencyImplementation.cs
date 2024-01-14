namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class DependencyImplementation : IDependency
{   //creates dependency occurance
    public int Create(Dependency item)
    {
        int id = DataSource.Config.NextDependencyId;
        Dependency newItem = item with { Id = id };
        DataSource.Dependencies.Add(newItem);
        return id;
    }

    //deletes a given dependency occurrence
    public void Delete(int id)
    {
        if (Read(id) is null)
            throw new Exception($"Dependency with ID={id} does not exists");
        DataSource.Dependencies.Remove(Read(id)!);
    }
    //prints all fields of a given dependency occurrence
    public Dependency? Read(int id)
    {
        return DataSource.Dependencies.Where(x => x.Id == id).FirstOrDefault();
    }
    //prints all fields of all ocurrences 
    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencies);
    }

    //updates an occurrence (the user enters vulues of all fields)
    public void Update(Dependency item)
    {
        if (Read(item.Id) is null)
            throw new Exception($"Dependency with ID={item.Id} does not exists");
        Delete(item.Id);
        DataSource.Dependencies.Add(item);
    }
}
