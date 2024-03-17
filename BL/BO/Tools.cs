using DO;
using System.Reflection;
using System.Xml.Serialization;
namespace BO;
public static class Tools
{
    private static BlApi.IBl _bl = BlApi.Factory.Get();
    const string s_xml_dir = @"..\xml\";
    public static BO.TaskInList GetTaskInList(int _id)
    {
        BO.Task boTask = _bl.Task.Read(_id) ?? throw new BO.BlDoesNotExistException($"task with id= {_id} doesn't exist");
        return new BO.TaskInList()
        {
            Id = _id,
            Alias = boTask.Alias,
            Description = boTask.Description,
            Status = boTask.Status
        };
    }



    public static void SaveListToXMLSerializer<T>(T list, string entity)
    {
        string filePath = $"{s_xml_dir + entity}.xml";
        try
        {
            using FileStream file = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            new XmlSerializer(typeof(T)).Serialize(file, list);
        }
        catch (Exception ex)
        {
            throw new DalXMLFileLoadCreateException($"fail to create xml file: {s_xml_dir + filePath}, {ex.Message}");
        }
    }

    public static string ToStringProperty<T>(this T t, string str = "")
    {
        foreach (PropertyInfo item in t.GetType().GetProperties())
        {
            if (item.GetValue(t, null) is IEnumerable<object>)
            {
                IEnumerable<object> list = (IEnumerable<object>)item.GetValue(obj: t, null);
                string s = string.Join("  ", list);
                str += "\n" + item.Name + ": " + s;
            }
            else
                str += "\n" + item.Name + ": " + item.GetValue(t, null);
        }
        return str;
    }
}

