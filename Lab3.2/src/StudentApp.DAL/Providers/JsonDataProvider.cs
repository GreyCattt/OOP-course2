using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace StudentApp.DAL.Providers
{
    public class JsonDataProvider<T> : IDataProvider<T> where T : class
    {
        public IEnumerable<T> Read(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<T>();
            }
            
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }

        public void Write(IEnumerable<T> data, string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(filePath, json);
        }
    }
}