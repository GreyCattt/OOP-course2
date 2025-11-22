using StoreApp.Core;

namespace StoreApp.BLL
{
    public class StoreService : IStoreService
    {
        private readonly ICategoryService _categoryService;
        private readonly ISupplierService _supplierService;
        private readonly ICustomerService _customerService;

        public StoreService(ICategoryService categoryService, ISupplierService supplierService, ICustomerService customerService)
        {
            _categoryService = categoryService;
            _supplierService = supplierService;
            _customerService = customerService;
        }

        public StoreService(ICategoryRepository categoryRepo, ISupplierRepository supplierRepo, ICustomerRepository customerRepo)
            : this(new CategoryService(categoryRepo), new SupplierService(supplierRepo), new CustomerService(customerRepo))
        {
        }
        public StoreService(StoreApp.DAL.IStorageService storageService)
            : this(new StoreApp.DAL.FileCategoryRepository(storageService), new StoreApp.DAL.FileSupplierRepository(storageService), new StoreApp.DAL.FileCustomerRepository(storageService))
        {
        }

        public void LoadData()
        {
        }

        public void SaveData()
        {
        }

        public Category AddCategory(string name)
        {
            return _categoryService.AddCategory(name);
        }

        public void RemoveCategory(int id)
        {
            _categoryService.RemoveCategory(id);
        }

        public void UpdateCategoryName(int id, string newName)
        {
            _categoryService.UpdateCategoryName(id, newName);
        }

        public Category GetCategoryById(int id)
        {
            return _categoryService.GetCategoryById(id);
        }

        public List<Category> GetAllCategories()
        {
            return _categoryService.GetAllCategories();
        }
        
        public (Product Product, Category Category) FindProductAndCategory(int productId)
        {
            return _categoryService.FindProductAndCategory(productId);
        }

        public Product AddProduct(int categoryId, string name, string brand, decimal price, int quantity)
        {
            return _categoryService.AddProduct(categoryId, name, brand, price, quantity);
        }

        public void RemoveProduct(int productId)
        {
            _categoryService.RemoveProduct(productId);
        }

        public Product UpdateProduct(int productId, string newName, string newBrand, decimal newPrice, int newQuantity)
        {
            return _categoryService.UpdateProduct(productId, newName, newBrand, newPrice, newQuantity);
        }

        public void UpdateProductQuantity(int productId, int newQuantity)
        {
            _categoryService.UpdateProductQuantity(productId, newQuantity);
        }

        public void ChangeProductCategory(int productId, int newCategoryId)
        {
            _categoryService.ChangeProductCategory(productId, newCategoryId);
        }

        public List<Product> GetAllProducts(string sortBy = "")
        {
            return _categoryService.GetAllProducts(sortBy);
        }
        public List<Product> SearchProducts(string keyword)
        {
            return _categoryService.SearchProducts(keyword);
        }


        public Supplier AddSupplier(string companyName, string firstName, string lastName, string phone)
        {
            return _supplierService.AddSupplier(companyName, firstName, lastName, phone);
        }

        public void RemoveSupplier(int id)
        {
            _supplierService.RemoveSupplier(id);
        }

        public Supplier GetSupplierById(int id)
        {
            return _supplierService.GetSupplierById(id);
        }
        
        public Supplier UpdateSupplier(int id, string companyName, string firstName, string lastName, string phone)
        {
            return _supplierService.UpdateSupplier(id, companyName, firstName, lastName, phone);
        }

        public List<Supplier> GetAllSuppliers(string sortBy = "")
        {
             return _supplierService.GetAllSuppliers(sortBy);
        }
        
        public Customer AddCustomer(string firstName, string lastName, string email, string phone)
        {
            return _customerService.AddCustomer(firstName, lastName, email, phone);
        }

        public Customer GetCustomerById(int id)
        {
             return _customerService.GetCustomerById(id);
        }

        public List<Customer> GetAllCustomers()
        {
            return _customerService.GetAllCustomers();
        }

        public List<Customer> SearchCustomers(string keyword)
        {
            return _customerService.SearchCustomers(keyword);
        }
    }
}