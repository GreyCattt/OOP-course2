using Moq;
using StudentApp.DAL.Entities;
using StudentApp.DAL.Providers;
using StudentApp.BLL.DTO;
using StudentApp.BLL.Services;
using StudentApp.BLL.Infrastructure;

namespace _5Lab_StudentApp.BLL.Test;

[TestClass]
public sealed class StudentServiceTests
{
    private Mock<IDataProvider<Student>>? _mockProvider;
    private StudentService? _studentService;

    private class TestStudentService : StudentService
    {
        private readonly IDataProvider<Student> _studentProvider;
        public TestStudentService(IDataProvider<Student> studentProvider)
        {
            _studentProvider = studentProvider;
        }

        protected override IDataProvider<T> GetProvider<T>(string providerType) where T : class
        {
            if (typeof(T) == typeof(Student))
            {
                return (IDataProvider<T>)_studentProvider;
            }
            return base.GetProvider<T>(providerType);
        }
    }

    [TestInitialize]
    public void Setup()
    {
        _mockProvider = new Mock<IDataProvider<Student>>();
        _studentService = new TestStudentService(_mockProvider.Object);
    }

    [TestMethod]
    public void AddNewStudent_ValidStudent_AddsSuccessfully()
    {
        var studentDto = new StudentDTO
        {
            FirstName = "John",
            LastName = "Doe",
            Course = 1,
            StudentCardId = "ST123456",
            CityOfArrival = "Kyiv"
        };
        var filePath = "test.json";
        var providerType = "json";

        _mockProvider!.Setup(p => p.Read(filePath)).Returns(new List<Student>());
        _mockProvider!.Setup(p => p.Write(It.IsAny<IEnumerable<Student>>(), filePath));

        _studentService!.AddNewStudent(studentDto, filePath, providerType);

        _mockProvider!.Verify(p => p.Write(It.IsAny<IEnumerable<Student>>(), filePath), Times.Once);
    }

    [TestMethod]
    public void AddNewStudent_Throws_When_FirstNameEmpty()
    {
        var studentDto = new StudentDTO { FirstName = "", LastName = "Doe", Course = 1, StudentCardId = "ID1", CityOfArrival = "Kyiv" };
        var filePath = "test.json";
        var providerType = "json";

        _mockProvider!.Setup(p => p.Read(filePath)).Returns(new List<Student>());

        Assert.ThrowsException<ValidationException>(() => _studentService!.AddNewStudent(studentDto, filePath, providerType));
    }

        [TestMethod]
        public void AddNewStudent_Throws_When_LastNameEmpty()
        {
            var studentDto = new StudentDTO { FirstName = "John", LastName = "", Course = 1, StudentCardId = "ID2", CityOfArrival = "Kyiv" };
            var filePath = "test.json";
            var providerType = "json";

            _mockProvider!.Setup(p => p.Read(filePath)).Returns(new List<Student>());

            Assert.ThrowsException<ValidationException>(() => _studentService!.AddNewStudent(studentDto, filePath, providerType));
        }

        [TestMethod]
        public void AddNewStudent_Throws_When_StudentCardIdEmpty()
        {
            var studentDto = new StudentDTO { FirstName = "John", LastName = "Doe", Course = 1, StudentCardId = "", CityOfArrival = "Kyiv" };
            var filePath = "test.json";
            var providerType = "json";

            _mockProvider!.Setup(p => p.Read(filePath)).Returns(new List<Student>());

            Assert.ThrowsException<ValidationException>(() => _studentService!.AddNewStudent(studentDto, filePath, providerType));
        }

        [TestMethod]
        public void AddNewStudent_Throws_When_CourseOutOfRange()
        {
            var filePath = "test.json";
            var providerType = "json";

            var studentDtoLow = new StudentDTO { FirstName = "John", LastName = "Doe", Course = 0, StudentCardId = "ID3", CityOfArrival = "Kyiv" };
            var studentDtoHigh = new StudentDTO { FirstName = "John", LastName = "Doe", Course = 7, StudentCardId = "ID4", CityOfArrival = "Kyiv" };

            _mockProvider!.Setup(p => p.Read(filePath)).Returns(new List<Student>());

            Assert.ThrowsException<ValidationException>(() => _studentService!.AddNewStudent(studentDtoLow, filePath, providerType));
            Assert.ThrowsException<ValidationException>(() => _studentService!.AddNewStudent(studentDtoHigh, filePath, providerType));
        }

    [TestMethod]
    public void AddNewStudent_Throws_When_DuplicateStudentCard()
    {
        var existing = new List<Student> { new Student { FirstName = "A", LastName = "B", Course = 2, StudentCardId = "DUP", CityOfArrival = "Kyiv" } };
        var studentDto = new StudentDTO { FirstName = "John", LastName = "Doe", Course = 2, StudentCardId = "DUP", CityOfArrival = "Kyiv" };
        var filePath = "test.json";
        var providerType = "json";

        _mockProvider!.Setup(p => p.Read(filePath)).Returns(existing);

        Assert.ThrowsException<ValidationException>(() => _studentService!.AddNewStudent(studentDto, filePath, providerType));
    }

    [TestMethod]
    public void GetAllStudents_Returns_MappedDTOs()
    {
        var students = new List<Student> { new Student { FirstName = "F", LastName = "L", Course = 3, StudentCardId = "S1", CityOfArrival = "Kyiv" } };
        var filePath = "test.json";
        var providerType = "json";
        _mockProvider!.Setup(p => p.Read(filePath)).Returns(students);

        var res = _studentService!.GetAllStudents(filePath, providerType).ToList();

        Assert.AreEqual(1, res.Count);
        Assert.AreEqual("F", res[0].FirstName);
        Assert.AreEqual("L", res[0].LastName);
        Assert.AreEqual(3, res[0].Course);
    }

    [TestMethod]
    public void CalculateFirstYearFromOtherCitiesPercentage_Throws_When_NoStudents()
    {
        var filePath = "test.json";
        var providerType = "json";
        _mockProvider!.Setup(p => p.Read(filePath)).Returns(new List<Student>());

        Assert.ThrowsException<StudentLogicException>(() => _studentService!.CalculateFirstYearFromOtherCitiesPercentage(filePath, providerType));
    }

    [TestMethod]
    public void CalculateFirstYearFromOtherCitiesPercentage_ReturnsZeroMessage_When_NoFirstYear()
    {
        var students = new List<Student> { new Student { FirstName = "A", LastName = "B", Course = 2, StudentCardId = "S1", CityOfArrival = "Lviv" } };
        var filePath = "test.json";
        var providerType = "json";
        _mockProvider!.Setup(p => p.Read(filePath)).Returns(students);

        var res = _studentService!.CalculateFirstYearFromOtherCitiesPercentage(filePath, providerType);

        Assert.IsTrue(res.Contains("Студентів 1-го курсу не знайдено"));
    }

}