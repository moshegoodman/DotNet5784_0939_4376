namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int id = DataSource.Config.NextDependencyId;
        Task newItem = item with { Id = id };
        DataSource.Tasks.Add(newItem);
        return id;
    }

    public void Delete(int id)
    {
        if (Read(id) is null)
            throw new Exception($"Dependency with ID={id} does not exists");
        DataSource.Tasks.Remove(Read(id)!);

    }

    public Task? Read(int id)
    {
        return DataSource.Tasks.Find(x => x.Id == id);

    }

    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }

    public void Update(Task item)
    {
        if (Read(item.Id) is null)
            throw new Exception($"Task with ID={item.Id} does not exists");
        Delete(item.Id);
        DataSource.Tasks.Add(item);
    }
}
