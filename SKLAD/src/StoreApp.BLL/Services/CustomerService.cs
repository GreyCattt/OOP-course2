using StoreApp.Core;

namespace StoreApp.BLL
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;

        public CustomerService(ICustomerRepository repo)
        {
            _repo = repo;
        }

        public Customer AddCustomer(string firstName, string lastName, string email, string phone)
        {
            return _repo.AddCustomer(firstName, lastName, email, phone);
        }

        public Customer GetCustomerById(int id)
        {
            return _repo.GetCustomerById(id);
        }

        public List<Customer> GetAllCustomers()
        {
            return _repo.GetAllCustomers();
        }

        public List<Customer> SearchCustomers(string keyword)
        {
            return _repo.SearchCustomers(keyword);
        }
    }
}
