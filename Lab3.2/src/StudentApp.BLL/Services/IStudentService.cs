using StudentApp.BLL.DTO;
using System.Collections.Generic;

namespace StudentApp.BLL.Services
{
    public interface IStudentService
    {
        void AddNewStudent(StudentDTO studentDto, string filePath, string providerType);
        IEnumerable<StudentDTO> GetAllStudents(string filePath, string providerType);
        string CalculateFirstYearFromOtherCitiesPercentage(string filePath, string providerType);
        
        void AddNewTaxiDriver(TaxiDriverDTO driverDto, string filePath, string providerType);
        IEnumerable<TaxiDriverDTO> GetAllTaxiDrivers(string filePath, string providerType);
        void AddNewAcrobat(AcrobatDTO acrobatDto, string filePath, string providerType);
        IEnumerable<AcrobatDTO> GetAllAcrobats(string filePath, string providerType);
    }
}