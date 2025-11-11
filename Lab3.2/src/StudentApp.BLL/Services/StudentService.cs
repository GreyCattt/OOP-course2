using StudentApp.BLL.DTO;
using StudentApp.BLL.Infrastructure;
using StudentApp.DAL;
using StudentApp.DAL.Entities;
using StudentApp.DAL.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentApp.BLL.Services
{
    public class StudentService : IStudentService
    {
        private IDataProvider<T> GetProvider<T>(string providerType) where T : class
        {
            switch (providerType.ToLower())
            {
                case "json":
                    return new JsonDataProvider<T>();
                case "xml":
                    return new XmlDataProvider<T>();
                default:
                    throw new StudentLogicException($"Провайдер типу '{providerType}' не підтримується.");
            }
        }

        private void ValidateStudent(StudentDTO dto, List<Student> existingStudents)
        {
            if (string.IsNullOrWhiteSpace(dto.FirstName))
                throw new ValidationException("Ім'я студента не може бути порожнім.");
            
            if (string.IsNullOrWhiteSpace(dto.LastName))
                throw new ValidationException("Прізвище студента не може бути порожнім.");

            if (string.IsNullOrWhiteSpace(dto.StudentCardId))
                throw new ValidationException("ID студ. квитка не може бути порожнім.");

            if (dto.Course < 1 || dto.Course > 6)
                throw new ValidationException("Курс має бути в діапазоні від 1 до 6.");
                
            if (existingStudents
                .Where(s => s != null) 
                .Any(s => s.StudentCardId.Equals(dto.StudentCardId, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ValidationException($"Студент з ID квитка '{dto.StudentCardId}' вже існує.");
            }

            if (existingStudents.Any(s => s.StudentCardId.Equals(dto.StudentCardId, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ValidationException($"Студент з ID квитка '{dto.StudentCardId}' вже існує.");
            }
        }

        public void AddNewStudent(StudentDTO studentDto, string filePath, string providerType)
        {
            var provider = GetProvider<Student>(providerType);
            var context = new EntityContext<Student>(filePath, provider);
            var students = context.LoadData().ToList();

            ValidateStudent(studentDto, students);

            var student = new Student
            {
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                Course = studentDto.Course,
                CityOfArrival = studentDto.CityOfArrival,
                StudentCardId = studentDto.StudentCardId
            };

            students.Add(student);
            context.SaveData(students);
        }

        public IEnumerable<StudentDTO> GetAllStudents(string filePath, string providerType)
        {
            var provider = GetProvider<Student>(providerType);
            var context = new EntityContext<Student>(filePath, provider);
            var students = context.LoadData();

            return students.Select(s => new StudentDTO
            {
                FirstName = s.FirstName,
                LastName = s.LastName,
                Course = s.Course,
                CityOfArrival = s.CityOfArrival,
                StudentCardId = s.StudentCardId
            });
        }
        

        public string CalculateFirstYearFromOtherCitiesPercentage(string filePath, string providerType)
        {
            var students = GetAllStudents(filePath, providerType).ToList();
            if (students.Count == 0)
                throw new StudentLogicException("Немає жодного студента у файлі для розрахунку.");

            string homeCity = "Київ";
            var firstYearStudents = students.Where(s => s.Course == 1).ToList();
            if (firstYearStudents.Count == 0)
                return "Студентів 1-го курсу не знайдено, відсоток 0%.";
            var fromOtherCities = firstYearStudents.Count(s => !s.CityOfArrival.Equals(homeCity, StringComparison.OrdinalIgnoreCase));
            double percentage = (double)fromOtherCities / firstYearStudents.Count * 100;
            return $"Відсоток студентів 1-го курсу з інших міст: {percentage:F2}% (Всього першокурсників: {firstYearStudents.Count}, з них з інших міст: {fromOtherCities})";
        }

        public void AddNewTaxiDriver(TaxiDriverDTO driverDto, string filePath, string providerType)
        {
            var provider = GetProvider<TaxiDriver>(providerType);
            var context = new EntityContext<TaxiDriver>(filePath, provider);
            var drivers = context.LoadData().ToList();
            var driver = new TaxiDriver { Name = driverDto.Name, CarModel = driverDto.CarModel };
            drivers.Add(driver);
            context.SaveData(drivers);
        }

        public IEnumerable<TaxiDriverDTO> GetAllTaxiDrivers(string filePath, string providerType)
        {
            var provider = GetProvider<TaxiDriver>(providerType);
            var context = new EntityContext<TaxiDriver>(filePath, provider);
            var drivers = context.LoadData();
            return drivers.Select(d => new TaxiDriverDTO { Name = d.Name, CarModel = d.CarModel });
        }

        public void AddNewAcrobat(AcrobatDTO acrobatDto, string filePath, string providerType)
        {
            var provider = GetProvider<Acrobat>(providerType);
            var context = new EntityContext<Acrobat>(filePath, provider);
            var acrobats = context.LoadData().ToList();
            var acrobat = new Acrobat { Nickname = acrobatDto.Nickname, ExperienceYears = acrobatDto.ExperienceYears };
            acrobats.Add(acrobat);
            context.SaveData(acrobats);
        }

        public IEnumerable<AcrobatDTO> GetAllAcrobats(string filePath, string providerType)
        {
            var provider = GetProvider<Acrobat>(providerType);
            var context = new EntityContext<Acrobat>(filePath, provider);
            var acrobats = context.LoadData();
            return acrobats.Select(a => new AcrobatDTO { Nickname = a.Nickname, ExperienceYears = a.ExperienceYears });
        }
    }
}