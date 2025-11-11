using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// Пригнічуємо попередження про застарілий BinaryFormatter
#pragma warning disable SYSLIB0011 
public class BinarySerializationStrategy : ISerializationStrategy
{
    public string Name => "Бінарний (BinaryFormatter з ISerializable)";
    private readonly BinaryFormatter _formatter = new BinaryFormatter();

    public void Serialize<T>(T obj, string filePath)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        {
            _formatter.Serialize(fs, obj);
        }
    }

    public T Deserialize<T>(string filePath)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Open))
        {
            return (T)_formatter.Deserialize(fs);
        }
    }
}
#pragma warning restore SYSLIB0011