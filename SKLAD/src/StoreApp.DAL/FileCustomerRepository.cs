using StoreApp.Core;
using StoreApp.Core.Exceptions;

namespace StoreApp.DAL
{
    public class FileCustomerRepository : ICustomerRepository
    {
        private readonly IStorageService _storage;

        public FileCustomerRepository(IStorageService storageService)
        {
            _storage = storageService;
        }

        public Customer AddCustomer(string firstName, string lastName, string email, string phone)
        {
            var ctx = _storage.LoadContext();
            int id = ctx.NextCustomerId++;
            var customer = new Customer(id, firstName, lastName, email, phone);
            ctx.Customers.Add(customer);
            _storage.SaveContext(ctx);
            return customer;
        }

        public Customer GetCustomerById(int id)
        {
            var ctx = _storage.LoadContext();
            var customer = ctx.Customers.FirstOrDefault(c => c.Id == id);
            if (customer == null) throw new EntityNotFoundException($"Замовника з ID {id} не знайдено.");
            return customer;
        }

        public List<Customer> GetAllCustomers()
        {
            var ctx = _storage.LoadContext();
            return ctx.Customers.ToList();
        }

        public List<Customer> SearchCustomers(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<Customer>();

            string lower = keyword.ToLower();
            var ctx = _storage.LoadContext();
            return ctx.Customers
                .Where(c => c.FirstName.ToLower().Contains(lower) || c.LastName.ToLower().Contains(lower) || c.Email.ToLower().Contains(lower))
                .ToList();
        }
    }
}
