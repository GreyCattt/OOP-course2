using StoreApp.Core;
using StoreApp.Core.Exceptions;

namespace StoreApp.DAL
{
    public class FileSupplierRepository : ISupplierRepository
    {
        private readonly IStorageService _storage;

        public FileSupplierRepository(IStorageService storageService)
        {
            _storage = storageService;
        }

        public Supplier AddSupplier(string companyName, string firstName, string lastName, string phone)
        {
            var ctx = _storage.LoadContext();
            int id = ctx.NextSupplierId++;
            var newSupplier = new Supplier(id, companyName, firstName, lastName, phone);
            ctx.Suppliers.Add(newSupplier);
            _storage.SaveContext(ctx);
            return newSupplier;
        }

        public void RemoveSupplier(int id)
        {
            var ctx = _storage.LoadContext();
            var supplier = ctx.Suppliers.FirstOrDefault(s => s.Id == id);
            if (supplier == null) throw new EntityNotFoundException($"Постачальника з ID {id} не знайдено.");
            ctx.Suppliers.Remove(supplier);
            _storage.SaveContext(ctx);
        }

        public Supplier GetSupplierById(int id)
        {
            var ctx = _storage.LoadContext();
            var supplier = ctx.Suppliers.FirstOrDefault(s => s.Id == id);
            if (supplier == null) throw new EntityNotFoundException($"Постачальника з ID {id} не знайдено.");
            return supplier;
        }

        public Supplier UpdateSupplier(int id, string companyName, string firstName, string lastName, string phone)
        {
            var ctx = _storage.LoadContext();
            var supplier = ctx.Suppliers.FirstOrDefault(s => s.Id == id);
            if (supplier == null) throw new EntityNotFoundException($"Постачальника з ID {id} не знайдено.");
            supplier.CompanyName = companyName;
            supplier.ContactFirstName = firstName;
            supplier.ContactLastName = lastName;
            supplier.Phone = phone;
            _storage.SaveContext(ctx);
            return supplier;
        }

        public List<Supplier> GetAllSuppliers(string sortBy = "")
        {
            var ctx = _storage.LoadContext();
            var suppliers = ctx.Suppliers.AsQueryable();
            switch (sortBy?.ToLower())
            {
                case "firstname":
                    suppliers = suppliers.OrderBy(s => s.ContactFirstName);
                    break;
                case "lastname":
                    suppliers = suppliers.OrderBy(s => s.ContactLastName);
                    break;
            }
            return suppliers.ToList();
        }
    }
}
