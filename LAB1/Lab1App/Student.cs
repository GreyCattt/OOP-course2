namespace LabProject
{
    public class Student : Person
    {
        public string Course { get; set; }
        public string StudentCard { get; set; }
        public string PassportSeriesNumber { get; set; }

        public string ArrivalCity { get; set; }

        public Student(string firstName, string lastName, string course, string studentCard, string passport, string arrivalCity)
            : base(firstName, lastName)
        {
            Course = course;
            StudentCard = studentCard;
            PassportSeriesNumber = passport;
            ArrivalCity = arrivalCity;
        }
        
        public override string GetInfo()
        {
            string danceStatus = CanDance ? "вміє танцювати" : "не вміє танцювати";
            return $"[СТУДЕНТ] {base.GetInfo()}, Курс: {Course}, Студентський: {StudentCard}, Паспорт: {PassportSeriesNumber}, Місто прибуття: {ArrivalCity ?? "Київ"}, {danceStatus}";
        }
    }
}