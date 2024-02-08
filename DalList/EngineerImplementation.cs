namespace Dal;
using DalApi;
using DO;

internal class EngineerImplementation : IEngineer
{
    //creates an engineer occurance
    public int Create(Engineer item)
    {
        if (Read(item.Id) is not null)
        {
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
        }
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    //deletes a given occurrence
    public void Delete(int id)
    {
        if (Read(id) is null)
            throw new DalDoesNotExistException($"Engineer with ID={id} does not exists");
        DataSource.Engineers.Remove(Read(id)!);
    }

    //prints all fields of a given engineer occurrence
    public Engineer? Read(int id)
    {
        return DataSource.Engineers.FirstOrDefault(item => item.Id == id);
    }

    //prints all fields of all ocurrences by condition
    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null)
    {

        if (filter != null)
        {
            return from item in DataSource.Engineers
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Engineers
               select item;
    }
    //updates an occurrence (the user enters vulues of all fields)
    public void Update(Engineer item)
    {
        if (Read(item.Id) is null)
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does not exists");
        Delete(item.Id);
        DataSource.Engineers.Add(item);
    }
    //Reads entity object by a given condition
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        return DataSource.Engineers.FirstOrDefault(item => filter(item));

    }

    public void Reset()
    {
        DataSource.Engineers.Clear();
    }
}
