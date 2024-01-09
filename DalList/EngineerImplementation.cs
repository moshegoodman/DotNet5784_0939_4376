namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class EngineerImplementation : IEngineer
{
    //creates an engineer occurance
    public int Create(Engineer item)
    {
        if (Read(item.Id) is not null)
        {
            throw new Exception($"Engineer with ID={item.Id} already exists");
        }
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    //deletes a given occurrence
    public void Delete(int id)
    {
        if (Read(id) is null)
            throw new Exception($"Engineer with ID={id} does not exists");
        DataSource.Engineers.Remove(Read(id)!);
    }

    //prints all fields of a given engineer occurrence
    public Engineer? Read(int id)
    {
        return DataSource.Engineers.Find(x => x.Id == id);
    }

    //prints all fields of all ocurrences 
    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }

    //updates an occurrence (the user enters vulues of all fields)
    public void Update(Engineer item)
    {
        if (Read(item.Id) is null)
            throw new Exception($"Engineer with ID={item.Id} does not exists");
        Delete(item.Id);
        DataSource.Engineers.Add(item);
    }
}
