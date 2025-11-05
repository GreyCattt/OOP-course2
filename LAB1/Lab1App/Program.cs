using System;
using System.IO;

namespace LabProject
{
    class Program
    {
        private const string UNIVERSITY_CITY = "Київ";
        private const string DATABASE_FILE = "database.txt";

        private static DatabaseService _db = new DatabaseService();
        private static FileService _fileService = new FileService(DATABASE_FILE);

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            
            try
            {
                _fileService.LoadData(_db);
                Console.WriteLine("Дані успішно завантажено з файлу.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка завантаження даних: {ex.Message}");
            }

            bool running = true;
            while (running)
            {
                ShowMenu();
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": AddNewStudent(); break;
                    case "2": AddNewTaxiDriver(); break;
                    case "3": AddNewAcrobat(); break;
                    case "4": ShowAllPeople(); break;
                    case "5": SearchByLastName(); break;
                    case "6": SearchByUniqueId(); break;
                    case "7": DeleteByUniqueId(); break;
                    case "8": RunVariantTask(); break;
                    case "9": SaveDataToFile(); break;
                    case "10": ClearDatabase(); break;
                    case "0": running = false; break;
                    default: Console.WriteLine("Невірний вибір."); break;
                }
                Console.WriteLine("\nНатисніть Enter для продовження...");
                Console.ReadLine();
                Console.Clear();
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("--- МЕНЮ КЕРУВАННЯ ---");
            Console.WriteLine("1. Додати Студента");
            Console.WriteLine("2. Додати Таксиста");
            Console.WriteLine("3. Додати Акробата");
            Console.WriteLine("4. Показати всіх");
            Console.WriteLine("5. Пошук за прізвищем");
            Console.WriteLine("6. Пошук за унікальним ID (студентський, номер водія...)");
            Console.WriteLine("7. Видалити за унікальним ID");
            Console.WriteLine("8. [ЗАВДАННЯ] Розрахувати % студентів 1-го курсу з інших міст");
            Console.WriteLine("9. Зберегти всі дані у файл");
            Console.WriteLine("10. Очистити всю базу даних");
            Console.WriteLine("0. Вихід");
            Console.Write("Ваш вибір: ");
        }

        static string ReadValidInput(string prompt, Func<string, bool> validator, string errorMessage)
        {
            string input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine().Trim();
                if (validator(input))
                {
                    return input;
                }
                Console.WriteLine(errorMessage);
            } while (true);
        }
        static string ReadOptionalInput(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine().Trim();
            return string.IsNullOrEmpty(input) ? null : input;
        }

        static void AddNewStudent()
        {
            Console.WriteLine("\n--- Додавання студента ---");
            string fn = ReadValidInput("Ім'я: ", ValidationService.IsValidName, "Некоректне ім'я.");
            string ln = ReadValidInput("Прізвище: ", ValidationService.IsValidName, "Некоректне прізвище.");
            string course = ReadValidInput("Курс (1-5): ", ValidationService.IsValidCourse, "Некоректний курс.");
            string card = ReadValidInput("Студентський (напр. KB123456): ", ValidationService.IsValidStudentCard, "Формат: 2 літери + 6 цифр.");
            string passport = ReadValidInput("Паспорт (напр. AA123456): ", ValidationService.IsValidPassport, "Формат: 2 літери + 6 цифр.");
            string city = ReadOptionalInput($"Місто прибуття (Enter, якщо з {UNIVERSITY_CITY}): ");
            Console.Write("Чи вміє танцювати? (так/ні): ");
            bool canDance = (Console.ReadLine()?.ToLower() ?? "") == "так";
            
            Student student = new Student(fn, ln, course, card, passport, city);
            student.CanDance = canDance;
            if (_db.AddStudent(student))
            {
                Console.WriteLine("Студента успішно додано.");
            }
            else
            {
                Console.WriteLine("Не вдалося додати студента. База даних повна або такий студентський квиток чи паспорт вже існує.");
            }
        }
        
        static void AddNewTaxiDriver()
        {
            Console.WriteLine("\n--- Додавання таксиста ---");
            string fn = ReadValidInput("Ім'я: ", ValidationService.IsValidName, "Некоректне ім'я.");
            string ln = ReadValidInput("Прізвище: ", ValidationService.IsValidName, "Некоректне прізвище.");
            Console.Write("Номер посвідчення: ");
            string license = Console.ReadLine() ?? "";
            Console.Write("Чи вміє танцювати? (так/ні): ");
            bool canDance = (Console.ReadLine()?.ToLower() ?? "") == "так";

            TaxiDriver driver = new TaxiDriver(fn, ln, license);
            driver.CanDance = canDance;
            if (_db.AddTaxiDriver(driver))
            {
                 Console.WriteLine("Таксиста успішно додано.");
            }
            else
            {
                Console.WriteLine("Не вдалося додати таксиста. База даних повна або такий номер посвідчення вже існує.");
            }
        }

        static void AddNewAcrobat()
        {
             Console.WriteLine("\n--- Додавання акробата ---");
            string fn = ReadValidInput("Ім'я: ", ValidationService.IsValidName, "Некоректне ім'я.");
            string ln = ReadValidInput("Прізвище: ", ValidationService.IsValidName, "Некоректне прізвище.");
            Console.Write("ID Перформера: ");
            string perfId = Console.ReadLine() ?? "";
            Console.Write("Чи вміє танцювати? (так/ні): ");
            bool canDance = (Console.ReadLine()?.ToLower() ?? "") == "так";

            Acrobat acrobat = new Acrobat(fn, ln, perfId);
            acrobat.CanDance = canDance;
            if (_db.AddAcrobat(acrobat))
            {
                 Console.WriteLine("Акробата успішно додано.");
            }
            else
            {
                Console.WriteLine("Не вдалося додати акробата. База даних повна або такий ID перформера вже існує.");
            }
        }


        static void ShowAllPeople()
        {
            Console.WriteLine("\n--- Список всіх осіб ---");
            Console.WriteLine("\n-- Студенти --");
            foreach (var s in _db.GetAllStudents())
            {
                Console.WriteLine(s.GetInfo());
            }
            
            Console.WriteLine("\n-- Таксисти --");
            foreach (var d in _db.GetAllTaxiDrivers())
            {
                Console.WriteLine(d.GetInfo());
            }

            Console.WriteLine("\n-- Акробати --");
            foreach (var a in _db.GetAllAcrobats())
            {
                Console.WriteLine(a.GetInfo());
            }
        }

        static void SearchByLastName()
        {
            Console.Write("Введіть прізвище для пошуку: ");
            string lastName = Console.ReadLine();
            Person person = _db.FindByLastName(lastName);
            if (person != null)
            {
                Console.WriteLine("Знайдено:");
                Console.WriteLine(person.GetInfo());
            }
            else
            {
                Console.WriteLine("Особу з таким прізвищем не знайдено.");
            }
        }

        static void SearchByUniqueId()
        {
            Console.Write("Введіть унікальний ID (студентський, ...): ");
            string id = Console.ReadLine();
            Person person = _db.FindByUniqueId(id);
            if (person != null)
            {
                Console.WriteLine("Знайдено:");
                Console.WriteLine(person.GetInfo());
            }
            else
            {
                Console.WriteLine("Особу з таким ID не знайдено.");
            }
        }

        static void DeleteByUniqueId()
        {
            Console.Write("Введіть ID особи для видалення: ");
            string id = Console.ReadLine();
            if (_db.DeleteByUniqueId(id))
            {
                Console.WriteLine("Особу успішно видалено.");
            }
            else
            {
                Console.WriteLine("Не вдалося знайти або видалити особу.");
            }
        }
        
        static void SaveDataToFile()
        {
            try
            {
                _fileService.SaveData(_db);
                Console.WriteLine("Дані успішно збережено у файл.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка збереження: {ex.Message}");
            }
        }

        static void ClearDatabase()
        {
            Console.WriteLine("Ви впевнені, що хочете очистити всю базу даних? (так/ні)");
            string response = Console.ReadLine()?.ToLower() ?? "";
            
            if (response == "так")
            {
                _db.ClearAll();
                Console.WriteLine("База даних успішно очищена.");
            }
            else
            {
                Console.WriteLine("Операцію скасовано.");
            }
        }
        static void RunVariantTask()
        {
            Console.WriteLine($"\n--- Завдання: Відсоток студентів 1-го курсу з інших міст (місто ВНЗ: {UNIVERSITY_CITY}) ---");

            Student[] allStudents = _db.GetAllStudents();
            if (allStudents.Length == 0)
            {
                Console.WriteLine("База даних студентів порожня.");
                return;
            }

            int totalFirstYears = 0;
            int firstYearsFromOtherCities = 0;

            Student[] targetStudents = new Student[allStudents.Length];
            int targetCount = 0;

            for (int i = 0; i < allStudents.Length; i++)
            {
                if (allStudents[i].Course == "1")
                {
                    totalFirstYears++;
                    
                    if (!string.IsNullOrEmpty(allStudents[i].ArrivalCity) &&
                        !allStudents[i].ArrivalCity.Equals(UNIVERSITY_CITY, StringComparison.OrdinalIgnoreCase))
                    {
                        firstYearsFromOtherCities++;
                        targetStudents[targetCount] = allStudents[i];
                        targetCount++;
                    }
                }
            }

            if (totalFirstYears == 0)
            {
                Console.WriteLine("Студентів 1-го курсу не знайдено.");
                return;
            }

            double percentage = (double)firstYearsFromOtherCities / totalFirstYears * 100.0;
            Console.WriteLine($"\nЗагальна кількість студентів 1-го курсу: {totalFirstYears}");
            Console.WriteLine($"Кількість студентів 1-го курсу з інших міст: {firstYearsFromOtherCities}");
            Console.WriteLine($"Відсоток: {percentage:F2}%");

            Console.WriteLine("\n--- Дані цих студентів ---");
            if (targetCount == 0)
            {
                Console.WriteLine("Студентів, що відповідають критерію, немає.");
            }
            else
            {
                for (int i = 0; i < targetCount; i++)
                {
                    Console.WriteLine(targetStudents[i].GetInfo());
                }
            }
        }
    }
}