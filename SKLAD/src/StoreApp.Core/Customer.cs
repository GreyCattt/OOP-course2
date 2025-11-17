namespace StoreApp.Core
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public Customer() { }

        public Customer(int id, string firstName, string lastName, string email, string phone)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
        }
        
        public override string ToString()
        {
            return $"ID: {Id}, Ім'я: {FirstName} {LastName}, Email: {Email}, Тел: {Phone}";
        }
    }
}