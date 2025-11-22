namespace StoreApp.Core
{
    public interface ICustomerRepository
    {
        Customer AddCustomer(string firstName, string lastName, string email, string phone);
        Customer GetCustomerById(int id);
        List<Customer> GetAllCustomers();
        List<Customer> SearchCustomers(string keyword);
    }
}
