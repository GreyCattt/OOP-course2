using StoreApp.Core;

namespace StoreApp.BLL
{
    public interface ISupplierService
    {
        Supplier AddSupplier(string companyName, string firstName, string lastName, string phone);
        void RemoveSupplier(int id);
        Supplier GetSupplierById(int id);
        Supplier UpdateSupplier(int id, string companyName, string firstName, string lastName, string phone);
        List<Supplier> GetAllSuppliers(string sortBy = "");
    }
}
