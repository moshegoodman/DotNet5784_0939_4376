namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;

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
            throw new DalDoesNotExistException($"Dependency with ID={id} does not exists");
        DataSource.Dependencies.Remove(Read(id)!);
    }
    //prints all fields of a given dependency occurrence
    public Dependency? Read(int id)
    {
        return DataSource.Dependencies.FirstOrDefault(item => item.Id == id);

    }
    //prints all fields of all ocurrences 
    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null)
    {
   
        if (filter != null)
        {
            return from item in DataSource.Dependencies
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Dependencies
               select item;

    }

    //updates an occurrence (the user enters vulues of all fields)
    public void Update(Dependency item)
    {
        if (Read(item.Id) is null)
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} does not exists");
        Delete(item.Id);
        DataSource.Dependencies.Add(item);
    }
    //Reads entity object by a given condition
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return DataSource.Dependencies.FirstOrDefault(item => filter(item));

    }

    
}
