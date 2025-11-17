using StoreApp.Core;
using StoreApp.Core.Exceptions;
using StoreApp.DAL;
using System.Collections.Generic;
using System.Linq;

namespace StoreApp.BLL
{
    public class StoreService
    {
        private readonly IStorageService _storageService;
        private DataContext _context;

        public StoreService(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public void LoadData()
        {
            _context = _storageService.LoadContext();
        }

        public void SaveData()
        {
            _storageService.SaveContext(_context);
        }

        public Category AddCategory(string name)
        {
            if (_context.Categories.Any(c => c.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase)))
            {
                throw new DuplicateItemException($"Категорія з назвою \"{name}\" вже існує.");
            }

            var category = new Category(_context.NextCategoryId++, name);
            _context.Categories.Add(category);
            return category;
        }

        public void RemoveCategory(int id)
        {
            var category = GetCategoryById(id);
            _context.Categories.Remove(category);
        }

        public void UpdateCategoryName(int id, string newName)
        {
            var category = GetCategoryById(id);
            category.Name = newName;
        }

        public Category GetCategoryById(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                throw new EntityNotFoundException($"Категорію з ID {id} не знайдено.");
            }
            return category;
        }

        public List<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }
        
        public (Product Product, Category Category) FindProductAndCategory(int productId)
        {
            foreach (var category in _context.Categories)
            {
                var product = category.Products.FirstOrDefault(p => p.Id == productId);
                if (product != null)
                {
                    return (product, category);
                }
            }
            throw new EntityNotFoundException($"Товар з ID {productId} не знайдено в жодній категорії.");
        }

        public Product AddProduct(int categoryId, string name, string brand, decimal price, int quantity)
        {
            var category = GetCategoryById(categoryId);
            var newProduct = new Product(_context.NextProductId++, name, brand, price, quantity);
            category.Products.Add(newProduct);
            return newProduct;
        }

        public void RemoveProduct(int productId)
        {
            var (product, category) = FindProductAndCategory(productId);
            category.Products.Remove(product);
        }

        public Product UpdateProduct(int productId, string newName, string newBrand, decimal newPrice, int newQuantity)
        {
            var (product, _) = FindProductAndCategory(productId);
            product.Name = newName;
            product.Brand = newBrand;
            product.Price = newPrice;
            product.Quantity = newQuantity;
            return product;
        }

        public void UpdateProductQuantity(int productId, int newQuantity)
        {
            var (product, _) = FindProductAndCategory(productId);
            product.Quantity = newQuantity;
        }

        public void ChangeProductCategory(int productId, int newCategoryId)
        {
            var (product, oldCategory) = FindProductAndCategory(productId);
            var newCategory = GetCategoryById(newCategoryId);

            if (oldCategory.Id == newCategory.Id)
            {
                throw new ValidationException("Товар вже знаходиться в цій категорії.");
            }

            oldCategory.Products.Remove(product);
            newCategory.Products.Add(product);
        }

        public List<Product> GetAllProducts(string sortBy = "")
        {
            var allProducts = _context.Categories.SelectMany(c => c.Products);

            switch (sortBy?.ToLower())
            {
                case "name":
                    allProducts = allProducts.OrderBy(p => p.Name);
                    break;
                case "brand":
                    allProducts = allProducts.OrderBy(p => p.Brand);
                    break;
                case "price_asc":
                    allProducts = allProducts.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    allProducts = allProducts.OrderByDescending(p => p.Price);
                    break;
            }
            return allProducts.ToList();
        }
        public List<Product> SearchProducts(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<Product>();
                
            string lowerKeyword = keyword.ToLower();
            
            return GetAllProducts()
                .Where(p => p.Name.ToLower().Contains(lowerKeyword) ||
                            p.Brand.ToLower().Contains(lowerKeyword))
                .ToList();
        }


        public Supplier AddSupplier(string companyName, string firstName, string lastName, string phone)
        {
            var newSupplier = new Supplier(_context.NextSupplierId++, companyName, firstName, lastName, phone);
            _context.Suppliers.Add(newSupplier);
            return newSupplier;
        }

        public void RemoveSupplier(int id)
        {
            var supplier = GetSupplierById(id);
            _context.Suppliers.Remove(supplier);
        }

        public Supplier GetSupplierById(int id)
        {
            var supplier = _context.Suppliers.FirstOrDefault(s => s.Id == id);
            if (supplier == null)
            {
                throw new EntityNotFoundException($"Постачальника з ID {id} не знайдено.");
            }
            return supplier;
        }
        
        public Supplier UpdateSupplier(int id, string companyName, string firstName, string lastName, string phone)
        {
            var supplier = GetSupplierById(id);
            supplier.CompanyName = companyName;
            supplier.ContactFirstName = firstName;
            supplier.ContactLastName = lastName;
            supplier.Phone = phone;
            return supplier;
        }

        public List<Supplier> GetAllSuppliers(string sortBy = "")
        {
             var suppliers = _context.Suppliers.AsQueryable();
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
        
        public Customer AddCustomer(string firstName, string lastName, string email, string phone)
        {
            var customer = new Customer(_context.NextCustomerId++, firstName, lastName, email, phone);
            _context.Customers.Add(customer);
            return customer;
        }

        public Customer GetCustomerById(int id)
        {
             var customer = _context.Customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                throw new EntityNotFoundException($"Замовника з ID {id} не знайдено.");
            }
            return customer;
        }

        public List<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }

        public List<Customer> SearchCustomers(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<Customer>();

            string lowerKeyword = keyword.ToLower();
            
            return _context.Customers
                .Where(c => c.FirstName.ToLower().Contains(lowerKeyword) ||
                            c.LastName.ToLower().Contains(lowerKeyword) ||
                            c.Email.ToLower().Contains(lowerKeyword))
                .ToList();
        }
    }
}