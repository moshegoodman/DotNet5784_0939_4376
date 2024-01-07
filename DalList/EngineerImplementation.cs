namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        if (Read(item.Id) is not null)
        {
            throw new Exception($"Engineer with ID={item.Id} already exists");
        }
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    public void Delete(int id)
    {
        if (Read(id) is null)
            throw new Exception($"Engineer with ID={id} does not exists");
        DataSource.Engineers.Remove(Read(id)!);
    }

    public Engineer? Read(int id)
    {
        return DataSource.Engineers.Find(x => x.Id == id);
    }

    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }

    public void Update(Engineer item)
    {
        if (Read(item.Id) is null)
            throw new Exception($"Engineer with ID={item.Id} does not exists");
        Delete(item.Id);
        DataSource.Engineers.Add(item);
    }
}
