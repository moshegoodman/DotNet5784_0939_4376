

namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    readonly string s_tasks_xml = "tasks";
    static string s_data_config_xml = "data-config";

    public int Create(Task item)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);

        int id = Config.NextTaskId;
        Task newItem = item with { Id = id };
        tasks.Add(newItem);
        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);

        return id;
    }

    //deletes a given task occurrence
    public void Delete(int id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);

        if (Read(id) is null)
            throw new DalDoesNotExistException($"Task with ID={id} does not exists");
        tasks.Remove(Read(id)!);
        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);

    }

    //prints all fields of a given task occurrence
    public Task? Read(int id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);

        return tasks.FirstOrDefault(item => item.Id == id);

    }

    //prints all fields of all ocurrences 
    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);

        if (filter != null)
        {
            return from item in tasks
                   where filter(item)
                   orderby item.Id
                   select item;
        }
        return from item in tasks
               orderby item.Id
               select item;
    }
    //updates an occurrence (the user enters vulues of all fields)
    public void Update(Task item)
    {

        if (Read(item.Id) is null)
            throw new DalDoesNotExistException($"Task with ID={item.Id} does not exists");
        Delete(item.Id);
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        tasks.Add(item);
        tasks = tasks.OrderBy(task => task.Id).ToList();
        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);

    }
    //Reads entity object by a given condition
    public Task? Read(Func<Task, bool> filter)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);

        return tasks.FirstOrDefault(item => filter(item));
    }

    public DateTime? GetStartDate()
    {
        return XMLTools.GetStartDate(s_data_config_xml);
    }

    public void SetStartDate(DateTime startDate)
    {
        XMLTools.SetStartDate(s_data_config_xml, startDate);
    }

    public int? GetStatus()
    {
        return XMLTools.GetStatus(s_data_config_xml);
    }

    public void IncreaseStatus()
    {
        XMLTools.IncreaseStatus(s_data_config_xml);
    }
}
