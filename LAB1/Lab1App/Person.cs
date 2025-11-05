namespace LabProject
{
    public abstract class Person : IDanceable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool CanDance { get; set; }

        protected Person(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            CanDance = false;
        }

        public virtual string GetInfo()
        {
            return $"Ім'я: {FirstName}, Прізвище: {LastName}";
        }
    }
}