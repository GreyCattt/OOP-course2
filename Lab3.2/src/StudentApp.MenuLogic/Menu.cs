using StudentApp.BLL.DTO;
using StudentApp.BLL.Infrastructure;
using StudentApp.BLL.Services;
using System;
using System.Collections.Generic;

namespace StudentApp.MenuLogic
{
    public class Menu
    {
        private readonly IStudentService _studentService;

        public Menu(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- ГОЛОВНЕ МЕНЮ ---");
                Console.WriteLine("--- Робота зі студентами ---");
                Console.WriteLine("1. Додати нового студента");
                Console.WriteLine("2. Показати всіх студентів");
                Console.WriteLine("3. Розрахувати відсоток студентів 1-го курсу");
                Console.WriteLine("--- Робота з сутностями ---");
                Console.WriteLine("4. Додати водія таксі");
                Console.WriteLine("5. Показати всіх водіїв"); 
                Console.WriteLine("6. Додати акробата");
                Console.WriteLine("7. Показати всіх акробатів");
                Console.WriteLine("0. Вихід");
                
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddNewStudent();
                        break;
                    case "2":
                        ShowAllStudents();
                        break;
                    case "3":
                        CalculatePercentage();
                        break;
                    case "4":
                        AddNewTaxiDriver();
                        break;
                    case "5":
                        ShowAllTaxiDrivers();
                        break;
                    case "6":
                        AddNewAcrobat();
                        break;
                    case "7":
                        ShowAllAcrobats();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Невірний вибір.");
                        break;
                }
                Console.WriteLine("\nНатисніть Enter для продовження...");
                Console.ReadLine();
            }
        }

        private (string filePath, string providerType) GetFileConfiguration()
        {
            Console.WriteLine("Введіть ім'я файлу (наприклад, students.json):");
            string fileName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(fileName)) fileName = "default.json";

            Console.WriteLine("Введіть тип серііалізації (json, xml):");
            string providerType = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(providerType)) providerType = "json";

            return (fileName, providerType);
        }

        private void AddNewStudent()
        {
            try
            {
                var (filePath, providerType) = GetFileConfiguration();
                var dto = new StudentDTO();

                do {
                    Console.WriteLine("Введіть Прізвище:");
                    dto.LastName = Console.ReadLine();
                } while (string.IsNullOrWhiteSpace(dto.LastName));
                
                do {
                    Console.WriteLine("Введіть Ім'я:");
                    dto.FirstName = Console.ReadLine();
                } while (string.IsNullOrWhiteSpace(dto.FirstName));
                
                do {
                    Console.WriteLine("Введіть ID студ. квитка (має бути унікальним!):");
                    dto.StudentCardId = Console.ReadLine();
                } while (string.IsNullOrWhiteSpace(dto.StudentCardId));

                int course;
                while (true)
                {
                    Console.WriteLine("Введіть Курс (число 1-6):");
                    if (int.TryParse(Console.ReadLine(), out course))
                    {
                        dto.Course = course;
                        break;
                    }
                    Console.WriteLine("Помилка: потрібно ввести число.");
                }
                
                Console.WriteLine("Введіть Місто прибуття (Enter = Київ):");
                dto.CityOfArrival = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(dto.CityOfArrival))
                {
                    dto.CityOfArrival = "Київ";
                    Console.WriteLine(" -> (встановлено 'Київ' за замовчуванням)");
                }

                _studentService.AddNewStudent(dto, filePath, providerType);
                Console.WriteLine("Студента успішно додано.");
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"[ПОМИЛКА ВАЛІДАЦІЇ]: {ex.Message}");
            }
            catch (StudentLogicException ex)
            {
                Console.WriteLine($"[ПОМИЛКА БІЗНЕС-ЛОГІКИ]: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ЗАГАЛЬНА ПОМИЛКА]: {ex.Message}");
            }
        }

        private void ShowAllStudents()
        {
            try
            {
                var (filePath, providerType) = GetFileConfiguration();
                var students = _studentService.GetAllStudents(filePath, providerType);
                
                int count = 0;
                foreach (var s in students)
                {
                    Console.WriteLine($"\nСтудент: {s.LastName} {s.FirstName} (Курс: {s.Course})");
                    Console.WriteLine($"  ID Квитка: {s.StudentCardId}, Місто: {s.CityOfArrival}");
                    count++;
                }
                Console.WriteLine($"\nВсього студентів у файлі: {count}");
            }
            catch (StudentLogicException ex) { Console.WriteLine($"[ПОМИЛКА БІЗНЕС-ЛОГІКИ]: {ex.Message}"); }
            catch (Exception ex) { Console.WriteLine($"[ЗАГАЛЬНА ПОМИЛКА]: {ex.Message}"); }
        }
        
        private void CalculatePercentage()
        {
             try
            {
                var (filePath, providerType) = GetFileConfiguration();
                string result = _studentService.CalculateFirstYearFromOtherCitiesPercentage(filePath, providerType);
                Console.WriteLine(result);
            }
            catch (StudentLogicException ex) { Console.WriteLine($"[ПОМИЛКА РОЗРАХУНКУ]: {ex.Message}"); }
            catch (Exception ex) { Console.WriteLine($"[ЗАГАЛЬНА ПОМИЛКА]: {ex.Message}"); }
        }


        private void AddNewTaxiDriver()
        {
            try
            {
                var (filePath, providerType) = GetFileConfiguration();
                var dto = new TaxiDriverDTO();
                Console.WriteLine("Введіть ім'я водія:");
                dto.Name = Console.ReadLine();
                Console.WriteLine("Введіть модель авто:");
                dto.CarModel = Console.ReadLine();
                _studentService.AddNewTaxiDriver(dto, filePath, providerType);
                Console.WriteLine("Водія таксі успішно додано.");
            }
            catch (StudentLogicException ex) { Console.WriteLine($"[ПОМИЛКА БІЗНЕС-ЛОГІКИ]: {ex.Message}"); }
            catch (Exception ex) { Console.WriteLine($"[ЗАГАЛЬНА ПОМИЛКА]: {ex.Message}"); }
        }

        private void ShowAllTaxiDrivers()
        {
            try
            {
                var (filePath, providerType) = GetFileConfiguration();
                var drivers = _studentService.GetAllTaxiDrivers(filePath, providerType);
                int count = 0;
                foreach (var d in drivers)
                {
                    Console.WriteLine($"\nВодій: {d.Name}, Авто: {d.CarModel}");
                    count++;
                }
                Console.WriteLine($"\nВсього водіїв у файлі: {count}");
            }
            catch (StudentLogicException ex) { Console.WriteLine($"[ПОМИЛКА БІЗНЕС-ЛОГІКИ]: {ex.Message}"); }
            catch (Exception ex) { Console.WriteLine($"[ЗАГАЛЬНА ПОМИЛКА]: {ex.Message}"); }
        }

        private void AddNewAcrobat()
        {
            try
            {
                var (filePath, providerType) = GetFileConfiguration();
                var dto = new AcrobatDTO();
                Console.WriteLine("Введіть псевдонім акробата:");
                dto.Nickname = Console.ReadLine();
                int years;
                while (true)
                {
                    Console.WriteLine("Введіть досвід (років):");
                    if (int.TryParse(Console.ReadLine(), out years) && years >= 0)
                    {
                        dto.ExperienceYears = years;
                        break;
                    }
                    Console.WriteLine("Помилка: введіть позитивне число.");
                }
                _studentService.AddNewAcrobat(dto, filePath, providerType);
                Console.WriteLine("Акробата успішно додано.");
            }
            catch (StudentLogicException ex) { Console.WriteLine($"[ПОМИЛКА БІЗНЕС-ЛОГІКИ]: {ex.Message}"); }
            catch (Exception ex) { Console.WriteLine($"[ЗАГАЛЬНА ПОМИЛКА]: {ex.Message}"); }
        }

        private void ShowAllAcrobats()
        {
             try
            {
                var (filePath, providerType) = GetFileConfiguration();
                var acrobats = _studentService.GetAllAcrobats(filePath, providerType);
                int count = 0;
                foreach (var a in acrobats)
                {
                    Console.WriteLine($"\nАкробат: {a.Nickname}, Досвід: {a.ExperienceYears} р.");
                    count++;
                }
                Console.WriteLine($"\nВсього акробатів у файлі: {count}");
            }
            catch (StudentLogicException ex) { Console.WriteLine($"[ПОМИЛКА БІЗНЕС-ЛОГІКИ]: {ex.Message}"); }
            catch (Exception ex) { Console.WriteLine($"[ЗАГАЛЬНА ПОМИЛКА]: {ex.Message}"); }
        }
    }
}