﻿namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

internal class DependencyImplementation : IDependency
{
    readonly string s_dependencies_xml = "dependencies";

    static string s_data_config_xml = "data-config";
    //creates dependency occurance
    public int Create(Dependency item)
    {

        XElement dependencies = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        int nextId = Config.NextDependencyId;
        XElement xDependency = new("Dependency");
        XElement id = new("Id", nextId);
        xDependency.Add(id);
        XElement dependentTask = new("DependentTask", item.DependentTask);
        xDependency.Add(dependentTask);
        XElement dependentOnTask = new("DependentOnTask", item.DependsOnTask);
        xDependency.Add(dependentOnTask);
        dependencies.Add(xDependency);
        XMLTools.SaveListToXMLElement(dependencies, s_dependencies_xml);
        return nextId;

    }

    //deletes a given dependency occurrence
    public void Delete(int id)
    {
        if (Read(id) is null)
            throw new DalDoesNotExistException($"Dependency with ID={id} does not exists");
        XElement? dependencies = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        dependencies.Elements().FirstOrDefault(item => (int?)item.Element("Id") == id)!.Remove();
        XMLTools.SaveListToXMLElement(dependencies,s_dependencies_xml);
    }
    //prints all fields of a given dependency occurrence


    public Dependency? Read(int id)
    {
        XElement? dependencies = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        XElement? xDependency = dependencies.Elements().FirstOrDefault(item => (int?)item.Element("Id") == id)?? null;
        return xDependency is null ? null : XMLTools.ToDependencyNullable(xDependency);
    }
    //prints all fields of all ocurrences 
    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null)
    {
        IEnumerable<XElement> dependencies = XMLTools.LoadListFromXMLElement(s_dependencies_xml).Elements();

        if (filter != null)
        { 
           return from item in dependencies
                  where filter(XMLTools.ToDependencyNullable(item)!)
                  select XMLTools.ToDependencyNullable(item);
        }
        return from item in dependencies
               select XMLTools.ToDependencyNullable(item);
    }

    //updates an occurrence (the user enters vulues of all fields)
    public void Update(Dependency item)
    {
        if (Read(item.Id) is null)
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} does not exists");
        Delete(item.Id);
        XElement dependencies = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        XElement xDependency = new("Dependency");
        XElement id = new("Id", item.Id);
        xDependency.Add(id);
        XElement dependentTask = new("DependentTask", item.DependentTask);
        xDependency.Add(dependentTask);
        XElement dependentOnTask = new("DependentOnTask", item.DependsOnTask);
        xDependency.Add(dependentOnTask);
        dependencies.Add(xDependency);
        IEnumerable<XElement> shortedlist = new List<XElement>() { dependencies }.OrderBy(xElement => (int?)xElement.Attribute("Id"));
        dependencies = new XElement(dependencies.Name, shortedlist.Select(x => x));
        XMLTools.SaveListToXMLElement(dependencies, s_dependencies_xml);

    }
    //Reads entity object by a given condition
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        XElement? dependencies = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        XElement? xDependency = dependencies.Elements().FirstOrDefault(item => filter(XMLTools.ToDependencyNullable(item)!));
        return xDependency is null ? null : XMLTools.ToDependencyNullable(xDependency);

    }

    public void Reset()
    {
        XElement dependencies = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        dependencies.RemoveAll();
        XMLTools.SaveListToXMLElement(dependencies, s_dependencies_xml);
        XMLTools.ResetId(s_data_config_xml, "NextDependencyId", 10000);
    }
}
