namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

internal class DependencyImplementation : IDependency
{
    readonly string s_dependencies_xml = "dependencies";
    //creates dependency occurance
    public int Create(Dependency item)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        int nextId = Config.NextDependencyId;
        Dependency copy = item with { Id = nextId };
        dependencies.Add(copy);

        XMLTools.SaveListToXMLSerializer(dependencies, s_dependencies_xml);
        return nextId;

    }

    //deletes a given dependency occurrence
    public void Delete(int id)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        if (Read(id) is null)
            throw new DalDoesNotExistException($"Dependency with ID={id} does not exists");
        dependencies.Remove(Read(id)!);
        XMLTools.SaveListToXMLSerializer(dependencies, s_dependencies_xml);
    }
    //prints all fields of a given dependency occurrence
    public Dependency? Read(int id)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        return dependencies.FirstOrDefault(item => item.Id == id);
    }
    //prints all fields of all ocurrences 
    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);

        if (filter != null)
        {
            return from item in dependencies
                   where filter(item)
                   select item;
        }
        return from item in dependencies
               select item;

    }

    //updates an occurrence (the user enters vulues of all fields)
    public void Update(Dependency item)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);

        if (Read(item.Id) is null)
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} does not exists");
        Delete(item.Id);
        dependencies.Add(item);
        XMLTools.SaveListToXMLSerializer(dependencies, s_dependencies_xml);

    }
    //Reads entity object by a given condition
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);

        return dependencies.FirstOrDefault(item => filter(item));

    }

    // returns true if the dependency already exists
    public bool DependencyExists(int dependentTask, int dependentOnTask)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);

        foreach (Dependency dependency in dependencies)
        {
            if (dependency.DependentTask == dependentTask && dependency.DependsOnTask == dependentOnTask)
                return true;
        }
        return false;
    }
}
