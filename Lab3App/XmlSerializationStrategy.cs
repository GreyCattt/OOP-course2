using System.IO;
using System.Xml.Serialization;

public class XmlSerializationStrategy : ISerializationStrategy
{
    public string Name => "XML (XmlSerializer)";

    public void Serialize<T>(T obj, string filePath)
    {
        // XmlSerializer потребує типу об'єкта при створенні
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        {
            xmlSerializer.Serialize(fs, obj);
        }
    }

    public T Deserialize<T>(string filePath)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        using (FileStream fs = new FileStream(filePath, FileMode.Open))
        {
            return (T)xmlSerializer.Deserialize(fs)!;
        }
    }
}