namespace StoreApp.Core
{
    public class Supplier
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string Phone { get; set; }
        
        public Supplier() { }

        public Supplier(int id, string companyName, string firstName, string lastName, string phone)
        {
            Id = id;
            CompanyName = companyName;
            ContactFirstName = firstName;
            ContactLastName = lastName;
            Phone = phone;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Компанія: \"{CompanyName}\", Контакт: {ContactFirstName} {ContactLastName}, Тел: {Phone}";
        }
    }
}