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
        return DataSource.Tasks.Find(x => x.Id == id);

    }

    //prints all fields of all ocurrences 
    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }

    //updates an occurrence (the user enters vulues of all fields)
    public void Update(Task item)
    {
        if (Read(item.Id) is null)
            throw new Exception($"Task with ID={item.Id} does not exists");
        Delete(item.Id);
        DataSource.Tasks.Add(item);
    }
}
