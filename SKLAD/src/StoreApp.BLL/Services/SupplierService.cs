using StoreApp.Core;

namespace StoreApp.BLL
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _repo;

        public SupplierService(ISupplierRepository repo)
        {
            _repo = repo;
        }

        public Supplier AddSupplier(string companyName, string firstName, string lastName, string phone)
        {
            return _repo.AddSupplier(companyName, firstName, lastName, phone);
        }

        public void RemoveSupplier(int id)
        {
            _repo.RemoveSupplier(id);
        }

        public Supplier GetSupplierById(int id)
        {
            return _repo.GetSupplierById(id);
        }

        public Supplier UpdateSupplier(int id, string companyName, string firstName, string lastName, string phone)
        {
            return _repo.UpdateSupplier(id, companyName, firstName, lastName, phone);
        }

        public List<Supplier> GetAllSuppliers(string sortBy = "")
        {
            return _repo.GetAllSuppliers(sortBy);
        }
    }
}
