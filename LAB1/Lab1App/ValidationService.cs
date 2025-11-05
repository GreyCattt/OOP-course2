using System.Text.RegularExpressions;

namespace LabProject
{
    public static class ValidationService
    {
        private static readonly Regex NameRegex = new Regex(@"^[A-Za-zА-Яа-яІіЇїЄє'-]+$");
        private static readonly Regex CourseRegex = new Regex(@"^[1-5]$");
        private static readonly Regex StudentCardRegex = new Regex(@"^[A-ZА-Я]{2}\d{6}$");
        private static readonly Regex PassportRegex = new Regex(@"^[A-ZА-Я]{2}\d{6}$");

        public static bool IsValidName(string name) => !string.IsNullOrWhiteSpace(name) && NameRegex.IsMatch(name);
        public static bool IsValidCourse(string course) => !string.IsNullOrWhiteSpace(course) && CourseRegex.IsMatch(course);
        public static bool IsValidStudentCard(string card) => !string.IsNullOrWhiteSpace(card) && StudentCardRegex.IsMatch(card);
        public static bool IsValidPassport(string passport) => !string.IsNullOrWhiteSpace(passport) && PassportRegex.IsMatch(passport);
    }
}