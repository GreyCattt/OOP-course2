using StoreApp.Core;

namespace StoreApp.DAL
{
    public class DataContext
    {
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Supplier> Suppliers { get; set; } = new List<Supplier>();
        public List<Customer> Customers { get; set; } = new List<Customer>();
        
        public int NextCategoryId { get; set; } = 1;
        public int NextProductId { get; set; } = 101;
        public int NextSupplierId { get; set; } = 1;
        public int NextCustomerId { get; set; } = 1;
    }
}