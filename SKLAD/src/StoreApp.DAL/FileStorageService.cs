using StoreApp.Core.Exceptions;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace StoreApp.DAL
{
    public class FileStorageService : IStorageService
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _jsonOptions;

        public FileStorageService(string filePath = "store_data.json")
        {
            _filePath = filePath;
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };
        }

        public DataContext LoadContext()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    return new DataContext();
                }

                string json = File.ReadAllText(_filePath);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return new DataContext();
                }

                return JsonSerializer.Deserialize<DataContext>(json, _jsonOptions) ?? new DataContext();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Помилка завантаження даних з файлу {_filePath}", ex);
            }
        }

        public void SaveContext(DataContext context)
        {
            try
            {
                string json = JsonSerializer.Serialize(context, _jsonOptions);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Помилка збереження даних у файл {_filePath}", ex);
            }
        }
    }
}