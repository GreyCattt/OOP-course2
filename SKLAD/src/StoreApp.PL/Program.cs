using StoreApp.BLL;
using StoreApp.Core;
using StoreApp.Core.Exceptions;
using StoreApp.DAL;

namespace StoreApp.PL
{
    public class Program
    {
        private static StoreService _storeService;

        public static void Main(string[] args)
        {
            IStorageService storage = new FileStorageService("store_data.json");
            
            _storeService = new StoreService(storage);

            try
            {
                _storeService.LoadData();
                Console.WriteLine("Дані успішно завантажено.");
            }
            catch (DataAccessException ex)
            {
                Console.WriteLine($"Критична помилка завантаження даних: {ex.Message}");
                Console.WriteLine("Додаток не може продовжити роботу.");
                return;
            }
            
            while (true)
            {
                ShowMenu();
                Console.Write("Ваш вибір: ");
                string choice = Console.ReadLine() ?? "";

                try
                {
                    bool shouldSave = HandleMenuChoice(choice);
                    
                    if (choice == "0") break;

                    if (shouldSave)
                    {
                        _storeService.SaveData();
                        Console.WriteLine("\n[Зміни успішно збережено у файл.]");
                    }
                }
                catch (EntityNotFoundException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\nПОМИЛКА: {ex.Message}");
                    Console.ResetColor();
                }
                catch (DuplicateItemException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\nПОМИЛКА: {ex.Message}");
                    Console.ResetColor();
                }
                 catch (ValidationException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\nПОМИЛКА ВАЛІДАЦІЇ: {ex.Message}");
                    Console.ResetColor();
                }
                catch (DataAccessException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nКРИТИЧНА ПОМИЛКА ДАНИХ: {ex.Message}");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nНЕОЧІКУВАНА ПОМИЛКА: {ex.Message}");
                    Console.ResetColor();
                }

                Console.WriteLine("\nНатисніть Enter для продовження...");
                Console.ReadLine();
            }
        }

        private static bool HandleMenuChoice(string choice)
        {
            switch (choice)
            {
                case "1": HandleAddCategory(); return true;
                case "2": HandleUpdateCategoryName(); return true;
                case "3": HandleRemoveCategory(); return true;
                case "4": HandleViewSingleCategory(); return false;
                case "5": HandleAddProduct(); return true;
                case "6": HandleRemoveProduct(); return true;
                case "7": HandleUpdateProduct(); return true;
                case "8": HandleUpdateProductQuantity(); return true;
                case "9": HandleChangeProductCategory(); return true;
                case "10": HandleAddSupplier(); return true;
                case "11": HandleUpdateSupplier(); return true;
                case "12": HandleRemoveSupplier(); return true;
                case "13": HandleViewSingleSupplier(); return false;
                case "14": HandleViewAllSuppliers(); return false;
                case "15": HandleAddCustomer(); return true;
                case "16": HandleViewSingleProduct(); return false;
                case "17": HandleViewAllProducts(); return false;
                case "18": DisplayAllStoreStructure(); return false;
                case "19": HandleSearchProducts(); return false;
                case "20": HandleSearchCustomers(); return false;
                case "21": HandleViewCustomerById(); return false;
                case "22": HandleViewAllCustomers(); return false;

                case "0":
                    Console.WriteLine("До побачення!");
                    return false;
                default:
                    Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                    return false;
            }
        }

        private static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════════╗");
            Console.WriteLine("║         МЕНЮ УПРАВЛІННЯ ТОВАРАМИ      ║");
            Console.WriteLine("╟───────────[ КАТЕГОРІЇ ]───────────────╢");
            Console.WriteLine("║ 1. Додати категорію                   ║");
            Console.WriteLine("║ 2. Змінити назву категорії            ║");
            Console.WriteLine("║ 3. Видалити категорію                 ║");
            Console.WriteLine("║ 4. Переглянути одну категорію         ║");
            Console.WriteLine("╟────────────[ ТОВАРИ ]─────────────────╢");
            Console.WriteLine("║ 5. Додати товар y категорію           ║");
            Console.WriteLine("║ 6. Видалити товар з категорії         ║");
            Console.WriteLine("║ 7. Змінити дані товару                ║");
            Console.WriteLine("║ 8. Змінити кількість товару           ║");
            Console.WriteLine("║ 9. Змінити категорію товару           ║");
            Console.WriteLine("╟─────────[ ПОСТАЧАЛЬНИКИ ]─────────────╢");
            Console.WriteLine("║ 10. Додати постачальника              ║");
            Console.WriteLine("║ 11. Змінити дані постачальника        ║");
            Console.WriteLine("║ 12. Видалити постачальника            ║");
            Console.WriteLine("║ 13. Переглянути постачальника         ║");
            Console.WriteLine("║ 14. Переглянути всіх постачальників   ║");
            Console.WriteLine("╟──────────[ ПЕРЕГЛЯД ТА ПОШУК ]────────╢");
            Console.WriteLine("║ 15. Додати замовника (для пошуку)     ║");
            Console.WriteLine("║ 16. Переглянути конкретний товар      ║"); 
            Console.WriteLine("║ 17. Переглянути всі товари (з сорт.)  ║"); 
            Console.WriteLine("║ 18. Показати все (структура)          ║");
            Console.WriteLine("║ 19. Пошук товару (за назвою/брендом)  ║");
            Console.WriteLine("║ 20. Пошук замовника (за ім'ям/email)  ║");
            Console.WriteLine("║ 21. Переглянути замовника за ID       ║");
            Console.WriteLine("║ 22. Переглянути всіх замовників       ║");
            Console.WriteLine("╟───────────────────────────────────────╢");
            Console.WriteLine("║ 0. Вийти (та зберегти)                ║");
            Console.WriteLine("╚═══════════════════════════════════════╝");
        }
        private static string ReadValidString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? "";
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }
                Console.WriteLine("Ввід не може бути порожнім. Спробуйте ще раз.");
            }
        }

        private static int ReadValidInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine() ?? "", out int result))
                {
                    return result;
                }
                Console.WriteLine("Некоректне число. Спробуйте ще раз.");
            }
        }
        
        private static int ReadValidInt(string prompt, int min)
        {
            while (true)
            {
                int result = ReadValidInt(prompt);
                if (result >= min)
                {
                    return result;
                }
                Console.WriteLine($"Число має бути {min} або більше. Спробуйте ще раз.");
            }
        }

        private static decimal ReadValidDecimal(string prompt, decimal min)
        {
             while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine() ?? "", out decimal result) && result >= min)
                {
                    return result;
                }
                Console.WriteLine($"Некоректна сума (має бути {min} або більше). Спробуйте ще раз.");
            }
        }
        

        private static void HandleAddCategory()
        {
            string name = ReadValidString("Введіть назву нової категорії: ");
            var newCategory = _storeService.AddCategory(name);
            Console.WriteLine($"Категорію \"{newCategory.Name}\" (ID: {newCategory.Id}) успішно створено.");
        }
        
        private static void HandleRemoveCategory()
        {
            int categoryId = ReadValidInt("Введіть ID категорії для видалення: ");
            _storeService.RemoveCategory(categoryId); 
            Console.WriteLine($"Категорію з ID {categoryId} видалено.");
        }

        private static void HandleUpdateCategoryName()
        {
            int categoryId = ReadValidInt("Введіть ID категорії для зміни: ");
            string newName = ReadValidString("Введіть нову назву: ");
            _storeService.UpdateCategoryName(categoryId, newName); 
            Console.WriteLine("Назву категорії оновлено.");
        }
        
        private static void HandleViewSingleCategory()
        {
            int categoryId = ReadValidInt("Введіть ID категорії: ");
            Category category = _storeService.GetCategoryById(categoryId); 
            
            Console.WriteLine($"\n--- Товари в категорії \"{category.Name}\" (ID: {category.Id}) ---");
            if (category.Products.Any())
            {
                foreach (var product in category.Products)
                {
                    Console.WriteLine(product);
                }
            }
            else
            {
                Console.WriteLine("Категорія порожня.");
            }
            Console.WriteLine("--------------------------------------");
        }

        private static void HandleAddProduct()
        {
            int categoryId = ReadValidInt("Введіть ID категорії, до якої додати товар: ");
            string name = ReadValidString("Введіть назву товару: ");
            string brand = ReadValidString("Введіть бренд товару: ");
            decimal price = ReadValidDecimal("Введіть ціну товару: ", 0);
            int quantity = ReadValidInt("Введіть кількість товару: ", 0);

            var newProduct = _storeService.AddProduct(categoryId, name, brand, price, quantity); 
            Console.WriteLine($"Товар \"{newProduct.Name}\" (ID: {newProduct.Id}) успішно додано.");
        }

        private static void HandleRemoveProduct()
        {
            int productId = ReadValidInt("Введіть ID товару для видалення: ");
            _storeService.RemoveProduct(productId);
            Console.WriteLine($"Товар з ID {productId} видалено.");
        }
        
        private static void HandleUpdateProduct()
        {
            int productId = ReadValidInt("Введіть ID товару для зміни: ");
            var (productToUpdate, _) = _storeService.FindProductAndCategory(productId);
            
            Console.WriteLine($"Поточні дані: {productToUpdate}");
            string newName = ReadValidString("Введіть нову назву: ");
            string newBrand = ReadValidString("Введіть новий бренд: ");
            decimal newPrice = ReadValidDecimal("Введіть нову ціну: ", 0);
            int newQuantity = ReadValidInt("Введіть нову кількість: ", 0);
            
            _storeService.UpdateProduct(productId, newName, newBrand, newPrice, newQuantity);
            Console.WriteLine("Дані товару оновлено.");
        }
        
        private static void HandleUpdateProductQuantity()
        {
            int productId = ReadValidInt("Введіть ID товару: ");
            int newQuantity = ReadValidInt("Введіть нову кількість: ", 0);
            
            _storeService.UpdateProductQuantity(productId, newQuantity);
            Console.WriteLine("Кількість товару оновлено.");
        }
        
        private static void HandleChangeProductCategory()
        {
            int productId = ReadValidInt("Введіть ID товару для переміщення: ");
            int newCategoryId = ReadValidInt("Введіть ID нової категорії: ");
            
            _storeService.ChangeProductCategory(productId, newCategoryId);
            Console.WriteLine("Товар успішно переміщено.");
        }
        
        private static void HandleViewSingleProduct()
        {
            int productId = ReadValidInt("Введіть ID товару: ");
            var (product, category) = _storeService.FindProductAndCategory(productId);
            
            Console.WriteLine("\n--- Детальна інформація про товар ---");
            Console.WriteLine(product);
            Console.WriteLine($"Належить до категорії: \"{category.Name}\" (ID: {category.Id})");
            Console.WriteLine("-------------------------------------");
        }

        private static void HandleViewAllProducts()
        {
            Console.WriteLine("\n--- Оберіть тип сортування ---");
            Console.WriteLine("1. За назвою (А-Я)");
            Console.WriteLine("2. За брендом (А-Я)");
            Console.WriteLine("3. За ціною (від дешевшого)");
            Console.WriteLine("4. За ціною (від дорожчого)");
            Console.WriteLine("Будь-яка інша клавіша - без сортування");
            Console.Write("Ваш вибір: ");
            string sortChoice = Console.ReadLine() ?? "";
            
            string sortBy = "";
            switch (sortChoice)
            {
                case "1": sortBy = "name"; break;
                case "2": sortBy = "brand"; break;
                case "3": sortBy = "price_asc"; break;
                case "4": sortBy = "price_desc"; break;
            }
            
            List<Product> allProducts = _storeService.GetAllProducts(sortBy);

            Console.WriteLine("\n--- Список всіх товарів ---");
            if (!allProducts.Any())
            {
                Console.WriteLine("У крамниці ще немає жодного товару.");
                return;
            }
            foreach (var product in allProducts)
            {
                Console.WriteLine(product);
            }
            Console.WriteLine("---------------------------");
        }

        private static void HandleAddSupplier()
        {
            Console.WriteLine("\n--- Додавання нового постачальника ---");
            string companyName = ReadValidString("Назва компанії: ");
            string firstName = ReadValidString("Ім'я контактної особи: ");
            string lastName = ReadValidString("Прізвище контактної особи: ");
            string phone = ReadValidString("Телефон: ");
            
            var newSupplier = _storeService.AddSupplier(companyName, firstName, lastName, phone);
            Console.WriteLine($"Постачальника \"{newSupplier.CompanyName}\" (ID: {newSupplier.Id}) додано.");
        }

        private static void HandleUpdateSupplier()
        {
            int supplierId = ReadValidInt("Введіть ID постачальника для зміни: ");
            var supplier = _storeService.GetSupplierById(supplierId);
            
            Console.WriteLine($"Поточні дані: {supplier}");
            string companyName = ReadValidString("Нова назва компанії: ");
            string firstName = ReadValidString("Нове ім'я контакту: ");
            string lastName = ReadValidString("Нове прізвище контакту: ");
            string phone = ReadValidString("Новий телефон: ");

            _storeService.UpdateSupplier(supplierId, companyName, firstName, lastName, phone);
            Console.WriteLine("Дані постачальника оновлено.");
        }
        
        private static void HandleRemoveSupplier()
        {
            int supplierId = ReadValidInt("Введіть ID постачальника для видалення: ");
            _storeService.RemoveSupplier(supplierId);
            Console.WriteLine($"Постачальника з ID {supplierId} видалено.");
        }

        private static void HandleViewSingleSupplier()
        {
            int supplierId = ReadValidInt("Введіть ID постачальника: ");
            var supplier = _storeService.GetSupplierById(supplierId);
            Console.WriteLine("\n--- Інформація про постачальника ---");
            Console.WriteLine(supplier);
            Console.WriteLine("-----------------------------------");
        }

        private static void HandleViewAllSuppliers()
        {
            Console.WriteLine("\n--- Сортувати список ---");
            Console.WriteLine("1. За ім'ям (А-Я)");
            Console.WriteLine("2. За прізвищем (А-Я)");
            Console.WriteLine("Будь-яка інша клавіша - без сортування");
            Console.Write("Ваш вибір: ");
            string sortChoice = Console.ReadLine() ?? "";

            string sortBy = "";
            if (sortChoice == "1") sortBy = "firstname";
            else if (sortChoice == "2") sortBy = "lastname";

            var suppliers = _storeService.GetAllSuppliers(sortBy);
            
            Console.WriteLine("\n--- Список всіх постачальників ---");
            if (!suppliers.Any())
            {
                Console.WriteLine("Список постачальників порожній.");
                return;
            }
            foreach (var supplier in suppliers)
            {
                Console.WriteLine(supplier);
            }
            Console.WriteLine("---------------------------------");
        }
        
        private static void HandleAddCustomer()
        {
            Console.WriteLine("\n--- Додавання нового замовника ---");
            string firstName = ReadValidString("Ім'я: ");
            string lastName = ReadValidString("Прізвище: ");
            string email = ReadValidString("Email: ");
            string phone = ReadValidString("Телефон: ");
            
            var customer = _storeService.AddCustomer(firstName, lastName, email, phone);
            Console.WriteLine($"Замовника {customer.FirstName} {customer.LastName} (ID: {customer.Id}) додано.");
        }

        private static void HandleSearchProducts()
        {
            string keyword = ReadValidString("Введіть ключове слово для пошуку (назва або бренд): ");
            var results = _storeService.SearchProducts(keyword);
            
            Console.WriteLine($"\n--- Результати пошуку ({results.Count} знайдено) ---");
            if (!results.Any())
            {
                Console.WriteLine("Нічого не знайдено.");
                return;
            }
            foreach (var product in results)
            {
                Console.WriteLine(product);
            }
            Console.WriteLine("---------------------------------------");
        }

        private static void HandleSearchCustomers()
        {
            string keyword = ReadValidString("Введіть ключове слово для пошуку (ім'я, прізвище, email): ");
            var results = _storeService.SearchCustomers(keyword);

            Console.WriteLine($"\n--- Результати пошуку ({results.Count} знайдено) ---");
            if (!results.Any())
            {
                Console.WriteLine("Нічого не знайдено.");
                return;
            }
            foreach (var customer in results)
            {
                Console.WriteLine(customer);
            }
            Console.WriteLine("---------------------------------------");
        }

        private static void HandleViewCustomerById()
        {
            int id = ReadValidInt("Введіть ID клієнта: ");
            var customer = _storeService.GetCustomerById(id);
            Console.WriteLine("\n--- Інформація про клієнта ---");
            Console.WriteLine(customer);
            Console.WriteLine("-------------------------------");
        }

        private static void HandleViewAllCustomers()
        {
            var customers = _storeService.GetAllCustomers();
            Console.WriteLine("\n--- Список всіх клієнтів ---");
            if (!customers.Any())
            {
                Console.WriteLine("Список клієнтів порожній.");
                return;
            }
            foreach (var c in customers)
            {
                Console.WriteLine(c);
            }
            Console.WriteLine("-------------------------------");
        }
        
        private static void DisplayAllStoreStructure()
        {
            Console.WriteLine("\n========== ВМІСТ КРАМНИЦІ ==========");
            var categories = _storeService.GetAllCategories();
            if (!categories.Any())
            {
                Console.WriteLine("У крамниці ще немає жодної категорії.");
            }
            
            foreach (var category in categories)
            {
                Console.WriteLine($"\nКатегорія (ID: {category.Id}): {category.Name}");
                Console.WriteLine("--------------------------------------");
                if (category.Products.Any())
                {
                    foreach (var product in category.Products)
                    {
                        Console.WriteLine($"  -> {product}");
                    }
                }
                else
                {
                    Console.WriteLine("  -> В категорії немає товарів.");
                }
            }
            Console.WriteLine("\n========================================\n");
        }
    }
}