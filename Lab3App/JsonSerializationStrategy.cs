using System.IO;
using System.Text.Json;

public class JsonSerializationStrategy : ISerializationStrategy
{
    public string Name => "JSON (System.Text.Json)";
    
    private readonly JsonSerializerOptions _options = 
        new JsonSerializerOptions { WriteIndented = true };

    public void Serialize<T>(T obj, string filePath)
    {
        string jsonString = JsonSerializer.Serialize(obj, _options);
        File.WriteAllText(filePath, jsonString);
    }

    public T Deserialize<T>(string filePath)
    {
        string jsonFromFile = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T>(jsonFromFile)!;
    }
}