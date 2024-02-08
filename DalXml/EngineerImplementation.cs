

namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;


internal class EngineerImplementation : IEngineer
{
    readonly string s_engineers_xml = "engineers";

    //creates an engineer occurance
    public int Create(Engineer item)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);

        if (Read(item.Id) is not null)
        {
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
        }
        engineers.Add(item);
        XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml);

        return item.Id;
    }

    //deletes a given occurrence
    public void Delete(int id)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);

        if (Read(id) is null)
            throw new DalDoesNotExistException($"Engineer with ID={id} does not exists");
        engineers.Remove(Read(id)!);
        XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml);

    }

    //prints all fields of a given engineer occurrence
    public Engineer? Read(int id)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);

        return engineers.FirstOrDefault(item => item.Id == id);
    }

    //prints all fields of all ocurrences by condition
    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);

        if (filter != null)
        {
            return from item in engineers
                   where filter(item)
                   select item;
        }
        return from item in engineers
               select item;
    }
    //updates an occurrence (the user enters vulues of all fields)
    public void Update(Engineer item)
    {
        

        if (Read(item.Id) is null)
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does not exists");
        Delete(item.Id);
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        engineers.Add(item);
        XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml);

    }
    //Reads entity object by a given condition
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);

        return engineers.FirstOrDefault(item => filter(item));

    }

    public void Reset()
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        engineers.Clear();
        XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml);
    }
}
