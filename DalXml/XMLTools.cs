namespace Dal;

using DO;
using System.Xml.Linq;
using System.Xml.Serialization;

static class XMLTools
{
    const string s_xml_dir = @"..\xml\";
    static XMLTools()
    {
        if (!Directory.Exists(s_xml_dir))
            Directory.CreateDirectory(s_xml_dir);
    }
    #region Convert Xelement-Dependency
    public static Dependency? ToDependencyNullable(XElement dependency)
    {
        return new Dependency(
            dependency.ToIntNullable("Id") ?? throw new FormatException("Can't convert id"),
            dependency.ToIntNullable("DependentTask") ?? null,
            dependency.ToIntNullable("DependentOnTask") ?? null);
    }
    #endregion

    #region Extension Fuctions
    public static T? ToEnumNullable<T>(this XElement element, string name) where T : struct, Enum =>
        Enum.TryParse<T>((string?)element.Element(name), out var result) ? (T?)result : null;
    public static DateTime? ToDateTimeNullable(this XElement element, string name) =>
        DateTime.TryParse((string?)element.Element(name), out var result) ? (DateTime?)result : null;
    public static double? ToDoubleNullable(this XElement element, string name) =>
        double.TryParse((string?)element.Element(name), out var result) ? (double?)result : null;
    public static int? ToIntNullable(this XElement element, string name) =>
        int.TryParse((string?)element.Element(name), out var result) ? (int?)result : null;
    #endregion

    #region XmlConfig
    public static int GetAndIncreaseNextId(string data_config_xml, string elemName)
    {
        XElement root = XMLTools.LoadListFromXMLElement(data_config_xml);
        int nextId = root.ToIntNullable(elemName) ?? throw new FormatException($"can't convert id.  {data_config_xml}, {elemName}");
        root.Element(elemName)?.SetValue((nextId + 1).ToString());
        XMLTools.SaveListToXMLElement(root, data_config_xml);
        return nextId;
    }

    public static void ResetId(string data_config_xml, string elemName, int resetedId)
    {
        XElement root = XMLTools.LoadListFromXMLElement(data_config_xml);
        root.Element(elemName)?.SetValue((resetedId).ToString());
        XMLTools.SaveListToXMLElement(root, data_config_xml);
    }

    public static int? GetStatus(string data_config_xml)
    {
        XElement root = XMLTools.LoadListFromXMLElement(data_config_xml);
        int? statusValue = root.ToIntNullable("Status");
        return statusValue;
    }

    public static void IncreaseStatus(string data_config_xml)
    {
        XElement root = XMLTools.LoadListFromXMLElement(data_config_xml);
        int? statusValue = root.ToIntNullable("Status");
        if (statusValue == null)
            statusValue = 1;
        else if (statusValue < 3)
            statusValue = statusValue.Value + 1;
        else
            return;
        root.Element("Status")?.SetValue((statusValue).ToString()!);
        XMLTools.SaveListToXMLElement(root, data_config_xml);
    }
    public static void NullifyStatus(string data_config_xml)
    {
        XElement root = XMLTools.LoadListFromXMLElement(data_config_xml);
        //int? statusValue = root.ToIntNullable("Status");
        int? statusValue = null;
        root.Element("Status")?.SetValue((statusValue).ToString()!);
        XMLTools.SaveListToXMLElement(root, data_config_xml);
    }

    public static DateTime? GetStartDate(string data_config_xml)
    {
        XElement root = XMLTools.LoadListFromXMLElement(data_config_xml);
        DateTime? startDate = root.ToDateTimeNullable("StartDate");
        return startDate;
    }

    public static void SetStartDate(string data_config_xml, DateTime startDate)
    {
        XElement root = XMLTools.LoadListFromXMLElement(data_config_xml);
        root.Element("StartDate")?.SetValue((startDate).ToString());
        XMLTools.SaveListToXMLElement(root, data_config_xml);
    }

    #endregion

    #region SaveLoadWithXElement
    public static void SaveListToXMLElement(XElement rootElem, string entity)
    {
        string filePath = $"{s_xml_dir + entity}.xml";
        try
        {
            rootElem.Save(filePath);
        }
        catch (Exception ex)
        {
            throw new DalXMLFileLoadCreateException($"fail to create xml file: {s_xml_dir + filePath}, {ex.Message}");
        }
    }
    public static XElement LoadListFromXMLElement(string entity)
    {
        string filePath = $"{s_xml_dir + entity}.xml";
        try
        {
            if (File.Exists(filePath))
                return XElement.Load(filePath);
            XElement rootElem = new(entity);
            rootElem.Save(filePath);
            return rootElem;
        }
        catch (Exception ex)
        {
            throw new DalXMLFileLoadCreateException($"fail to load xml file: {s_xml_dir + filePath}, {ex.Message}");
        }
    }
    #endregion

    #region SaveLoadWithXMLSerializer
    public static void SaveListToXMLSerializer<T>(List<T> list, string entity) where T : class
    {
        string filePath = $"{s_xml_dir + entity}.xml";
        try
        {
            using FileStream file = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            new XmlSerializer(typeof(List<T>)).Serialize(file, list);
        }
        catch (Exception ex)
        {
            throw new DalXMLFileLoadCreateException($"fail to create xml file: {s_xml_dir + filePath}, {ex.Message}");
        }
    }
    public static List<T> LoadListFromXMLSerializer<T>(string entity) where T : class
    {
        string filePath = $"{s_xml_dir + entity}.xml";
        try
        {
            if (!File.Exists(filePath)) return new();
            using FileStream file = new(filePath, FileMode.Open);
            XmlSerializer x = new(typeof(List<T>));
            return x.Deserialize(file) as List<T> ?? new();
        }
        catch (Exception ex)
        {
            throw new DalXMLFileLoadCreateException($"fail to load xml file: {filePath}, {ex.Message}");
        }
    }
    #endregion
}

