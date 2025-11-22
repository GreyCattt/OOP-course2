using StoreApp.Core.Exceptions;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace StoreApp.DAL
{
    public class FileStorageService : IStorageService
    {
        private readonly string _directoryPath;
        private readonly JsonSerializerOptions _jsonOptions;

        public FileStorageService(string filePath = "store_data.json")
        {
            if (Path.HasExtension(filePath))
            {
                _directoryPath = Path.GetDirectoryName(Path.GetFullPath(filePath)) ?? Directory.GetCurrentDirectory();
            }
            else
            {
                _directoryPath = Path.GetFullPath(filePath);
            }

            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };
        }

        private string CategoriesFile => Path.Combine(_directoryPath, "categories_data.json");
        private string SuppliersFile => Path.Combine(_directoryPath, "suppliers_data.json");
        private string CustomersFile => Path.Combine(_directoryPath, "customers_data.json");
        private string IdsFile => Path.Combine(_directoryPath, "ids_data.json");

        public DataContext LoadContext()
        {
            try
            {
                Directory.CreateDirectory(_directoryPath);

                var context = new DataContext();

                if (File.Exists(CategoriesFile))
                {
                    string json = File.ReadAllText(CategoriesFile);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var cats = JsonSerializer.Deserialize<List<StoreApp.Core.Category>>(json, _jsonOptions);
                        if (cats != null)
                            context.Categories = cats;
                    }
                }

                if (File.Exists(SuppliersFile))
                {
                    string json = File.ReadAllText(SuppliersFile);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var sup = JsonSerializer.Deserialize<List<StoreApp.Core.Supplier>>(json, _jsonOptions);
                        if (sup != null)
                            context.Suppliers = sup;
                    }
                }

                if (File.Exists(CustomersFile))
                {
                    string json = File.ReadAllText(CustomersFile);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var cust = JsonSerializer.Deserialize<List<StoreApp.Core.Customer>>(json, _jsonOptions);
                        if (cust != null)
                            context.Customers = cust;
                    }
                }

                if (File.Exists(IdsFile))
                {
                    string json = File.ReadAllText(IdsFile);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var ids = JsonSerializer.Deserialize<IdsData>(json, _jsonOptions);
                        if (ids != null)
                        {
                            context.NextCategoryId = ids.NextCategoryId;
                            context.NextProductId = ids.NextProductId;
                            context.NextSupplierId = ids.NextSupplierId;
                            context.NextCustomerId = ids.NextCustomerId;
                        }
                    }
                }
                else
                {
                    context.NextCategoryId = context.Categories.Any() ? context.Categories.Max(c => c.Id) + 1 : 1;
                    int maxProductId = context.Categories.SelectMany(c => c.Products ?? new List<StoreApp.Core.Product>())
                        .Select(p => p.Id).DefaultIfEmpty(100).Max();
                    context.NextProductId = maxProductId + 1;
                    context.NextSupplierId = context.Suppliers.Any() ? context.Suppliers.Max(s => s.Id) + 1 : 1;
                    context.NextCustomerId = context.Customers.Any() ? context.Customers.Max(c => c.Id) + 1 : 1;
                }

                return context;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Помилка завантаження даних з теки {_directoryPath}", ex);
            }
        }

        public void SaveContext(DataContext context)
        {
            try
            {
                Directory.CreateDirectory(_directoryPath);

                string catsJson = JsonSerializer.Serialize(context.Categories ?? new List<StoreApp.Core.Category>(), _jsonOptions);
                File.WriteAllText(CategoriesFile, catsJson);

                string supJson = JsonSerializer.Serialize(context.Suppliers ?? new List<StoreApp.Core.Supplier>(), _jsonOptions);
                File.WriteAllText(SuppliersFile, supJson);

                string custJson = JsonSerializer.Serialize(context.Customers ?? new List<StoreApp.Core.Customer>(), _jsonOptions);
                File.WriteAllText(CustomersFile, custJson);

                var ids = new IdsData
                {
                    NextCategoryId = context.NextCategoryId,
                    NextProductId = context.NextProductId,
                    NextSupplierId = context.NextSupplierId,
                    NextCustomerId = context.NextCustomerId
                };
                string idsJson = JsonSerializer.Serialize(ids, _jsonOptions);
                File.WriteAllText(IdsFile, idsJson);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Помилка збереження даних у теку {_directoryPath}", ex);
            }
        }

        private class IdsData
        {
            public int NextCategoryId { get; set; }
            public int NextProductId { get; set; }
            public int NextSupplierId { get; set; }
            public int NextCustomerId { get; set; }
        }
    }
}