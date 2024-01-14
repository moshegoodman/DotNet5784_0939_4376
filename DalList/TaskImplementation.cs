namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{

    //creates task occurance
    public int Create(Task item)
    {
        int id = DataSource.Config.NextTaskId;
        Task newItem = item with { Id = id };
        DataSource.Tasks.Add(newItem);
        return id;
    }

    //deletes a given task occurrence
    public void Delete(int id)
    {
        if (Read(id) is null)
            throw new Exception($"Task with ID={id} does not exists");
        DataSource.Tasks.Remove(Read(id)!);

    }

    //prints all fields of a given task occurrence
    public Task? Read(int id)
    {
        return DataSource.Tasks.FirstOrDefault(item => item.Id == id);

    }

    //prints all fields of all ocurrences 
    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null)
    {

        if (filter != null)
        {
            return from item in DataSource.Tasks
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Tasks
               select item;
    }
    //updates an occurrence (the user enters vulues of all fields)
    public void Update(Task item)
    {
        if (Read(item.Id) is null)
            throw new Exception($"Task with ID={item.Id} does not exists");
        Delete(item.Id);
        DataSource.Tasks.Add(item);
    }
    //Reads entity object by a given condition
    public Task? Read(Func<Task, bool> filter)
    {
        return DataSource.Tasks.FirstOrDefault(item => filter(item));

    }
}
