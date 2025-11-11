namespace StudentApp.DAL.Entities
{
    public class Student
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Course { get; set; }
        public string StudentCardId { get; set; }
        public string CityOfArrival { get; set; }
        public string PassportSeries { get; set; }
    }
}