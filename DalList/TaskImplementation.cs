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
            throw new DalDoesNotExistException($"Task with ID={id} does not exists");
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
                   orderby item.Id
                   select item;
        }
        return from item in DataSource.Tasks
               orderby item.Id
               select item;
    }
    //updates an occurrence (the user enters vulues of all fields)
    public void Update(Task item)
    {
        if (Read(item.Id) is null)
            throw new DalDoesNotExistException($"Task with ID={item.Id} does not exists");
        Delete(item.Id);
        int index = DataSource.Tasks.FindIndex(task => task.Id >= item.Id);

        // If index is negative, it means the newItem is greater than all elements in the list
        if (index < 0)
        {
            DataSource.Tasks.Add(item);
        }
        else
        {
            DataSource.Tasks.Insert(index, item); // Insert the new item at the appropriate position
        }
    }
    //Reads entity object by a given condition
    public Task? Read(Func<Task, bool> filter)
    {
        return DataSource.Tasks.FirstOrDefault(item => filter(item));

    }

    public DateTime? GetStartDate()
    {
        return DataSource.Config.StartDate;
    }

    public void SetStartDate(DateTime startDate)
    {
        DataSource.Config.StartDate = startDate;
    }

    public int? GetStatus()
    {
        return DataSource.Config.Status;
    }

    public void IncreaseStatus()
    {
        if (DataSource.Config.Status == null)
            DataSource.Config.Status = 1;
        else
            DataSource.Config.Status += 1;
    }

    public void Reset()
    {
        DataSource.Tasks.Clear();
    }
}
