using StoreApp.Core.Exceptions;
using StoreApp.DAL;

namespace StoreApp.BLL.Tests
{
    [TestClass]
    public class StoreServiceTests
    {
        private StoreService _storeService;
        private FakeStorageService _fakeStorage;
        private DataContext _context;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeStorage = new FakeStorageService();
            _context = _fakeStorage.Context;
            
            _storeService = new StoreService(_fakeStorage);
            _storeService.LoadData();
        }

        [TestMethod]
        public void AddCategory_ShouldSucceed_WhenNameIsUnique()
        {
            string categoryName = "Електроніка";

            var newCategory = _storeService.AddCategory(categoryName);

            Assert.IsNotNull(newCategory);
            Assert.AreEqual(1, _context.Categories.Count);
            Assert.AreEqual(categoryName, _context.Categories.First().Name);
            Assert.AreEqual(1, newCategory.Id); 
            Assert.AreEqual(2, _context.NextCategoryId);
        }

        [TestMethod]
        public void AddCategory_ShouldThrowDuplicateItemException_WhenNameExists()
        {
            _storeService.AddCategory("Електроніка");

            Assert.ThrowsException<DuplicateItemException>(() =>
            {
                _storeService.AddCategory("електроніка");
            });
        }

        [TestMethod]
        public void GetCategoryById_ShouldReturnCategory_WhenExists()
        {
            var addedCategory = _storeService.AddCategory("Електроніка");
            int id = addedCategory.Id;
            var foundCategory = _storeService.GetCategoryById(id);

            Assert.IsNotNull(foundCategory);
            Assert.AreEqual(id, foundCategory.Id);
            Assert.AreEqual("Електроніка", foundCategory.Name);
        }

        [TestMethod]
        public void GetCategoryById_ShouldThrowEntityNotFoundException_WhenDoesNotExist()
        {
            int fakeId = 99;

            Assert.ThrowsException<EntityNotFoundException>(() =>
            {
                _storeService.GetCategoryById(fakeId);
            });
        }

        [TestMethod]
        public void RemoveCategory_ShouldSucceed_WhenCategoryExists()
        {
            var addedCategory = _storeService.AddCategory("Електроніка");
            Assert.AreEqual(1, _context.Categories.Count);

            _storeService.RemoveCategory(addedCategory.Id);

            Assert.AreEqual(0, _context.Categories.Count);
        }

        [TestMethod]
        public void RemoveCategory_ShouldThrowEntityNotFoundException_WhenCategoryDoesNotExist()
        {
            int fakeId = 99;

            Assert.ThrowsException<EntityNotFoundException>(() =>
            {
                _storeService.RemoveCategory(fakeId);
            });
        }

        [TestMethod]
        public void UpdateCategoryName_ShouldSucceed_WhenCategoryExists()
        {
            var category = _storeService.AddCategory("Стара Назва");
            string newName = "Нова Назва";

            _storeService.UpdateCategoryName(category.Id, newName);

            var updatedCategory = _storeService.GetCategoryById(category.Id);
            Assert.AreEqual(newName, updatedCategory.Name);
        }
        
        [TestMethod]
        public void GetAllCategories_ShouldReturnAllCategories()
        {
            _storeService.AddCategory("Категорія 1");
            _storeService.AddCategory("Категорія 2");

            var categories = _storeService.GetAllCategories();

            Assert.AreEqual(2, categories.Count);
        }

        [TestMethod]
        public void AddProduct_ShouldSucceed_WhenCategoryExists()
        {
            var category = _storeService.AddCategory("Ноутбуки");
            int categoryId = category.Id;
            var product = _storeService.AddProduct(categoryId, "MacBook Pro", "Apple", 1500, 10);

            Assert.AreEqual(101, product.Id);
            Assert.AreEqual(1, category.Products.Count);
            Assert.AreEqual("MacBook Pro", category.Products.First().Name);
        }

        [TestMethod]
        public void AddProduct_ShouldThrowEntityNotFoundException_WhenCategoryDoesNotExist()
        {
            int fakeCategoryId = 99;
            Assert.ThrowsException<EntityNotFoundException>(() =>
            {
                _storeService.AddProduct(fakeCategoryId, "MacBook Pro", "Apple", 1500, 10);
            });
        }
        
        [TestMethod]
        public void ChangeProductCategory_ShouldSucceed_WhenCategoriesAreDifferent()
        {
            var oldCategory = _storeService.AddCategory("Ноутбуки");
            var newCategory = _storeService.AddCategory("Розпродаж");
            var product = _storeService.AddProduct(oldCategory.Id, "MacBook Pro", "Apple", 1500, 10);

            _storeService.ChangeProductCategory(product.Id, newCategory.Id);

            Assert.AreEqual(0, oldCategory.Products.Count);
            Assert.AreEqual(1, newCategory.Products.Count);
            Assert.AreEqual(product.Id, newCategory.Products.First().Id);
        }

        [TestMethod]
        public void ChangeProductCategory_ShouldThrowValidationException_WhenCategoriesAreSame()
        {
            var category = _storeService.AddCategory("Ноутбуки");
            var product = _storeService.AddProduct(category.Id, "MacBook Pro", "Apple", 1500, 10);
            
            Assert.ThrowsException<ValidationException>(() =>
            {
                 _storeService.ChangeProductCategory(product.Id, category.Id);
            });
        }
        
        [TestMethod]
        public void GetAllProducts_ShouldReturnSortedByName()
        {
            var category = _storeService.AddCategory("Ноутбуки");
            _storeService.AddProduct(category.Id, "ZenBook", "Asus", 1200, 5);
            _storeService.AddProduct(category.Id, "MacBook Pro", "Apple", 1500, 10);
            _storeService.AddProduct(category.Id, "Dell XPS", "Dell", 1300, 8);

            var products = _storeService.GetAllProducts("name");

            Assert.AreEqual("Dell XPS", products[0].Name);
            Assert.AreEqual("MacBook Pro", products[1].Name);
            Assert.AreEqual("ZenBook", products[2].Name);
        }
        
        [TestMethod]
        public void AddSupplier_ShouldSucceed()
        {
 
            var supplier = _storeService.AddSupplier("IT-World", "Іван", "Петренко", "0501234567");

            Assert.AreEqual(1, _context.Suppliers.Count);
            Assert.AreEqual(1, supplier.Id);
            Assert.AreEqual("IT-World", supplier.CompanyName);
        }

        [TestMethod]
        public void GetSupplierById_ShouldReturnSupplier_WhenExists()
        {
            var supplier = _storeService.AddSupplier("IT-World", "Іван", "Петренко", "0501234567");
            var found = _storeService.GetSupplierById(supplier.Id);
            
            Assert.IsNotNull(found);
            Assert.AreEqual(supplier.Id, found.Id);
        }

        [TestMethod]
        public void GetSupplierById_ShouldThrowEntityNotFoundException_WhenDoesNotExist()
        {
            int fakeId = 99;
            
            Assert.ThrowsException<EntityNotFoundException>(() =>
            {
                _storeService.GetSupplierById(fakeId);
            });
        }

        [TestMethod]
        public void AddCustomer_ShouldSucceed()
        {
            var customer = _storeService.AddCustomer("Марія", "Іванова", "m.ivanova@gmail.com", "0997654321");
            
            Assert.AreEqual(1, _context.Customers.Count);
            Assert.AreEqual(1, customer.Id);
            Assert.AreEqual("Марія", customer.FirstName);
        }

        [TestMethod]
        public void SearchCustomers_ShouldReturnMatchingByEmail()
        {
            _storeService.AddCustomer("Марія", "Іванова", "m.ivanova@gmail.com", "0997654321");
            _storeService.AddCustomer("Петро", "Сидоренко", "p.sydorenko@yahoo.com", "0671112233");
            _storeService.AddCustomer("Ольга", "Коваль", "olga.koval@gmail.com", "0504445566");

            var results = _storeService.SearchCustomers("gmail.com");


            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(results.Any(c => c.FirstName == "Марія"));
            Assert.IsTrue(results.Any(c => c.FirstName == "Ольга"));
        }

        [TestMethod]
        public void SearchCustomers_ShouldReturnEmpty_WhenKeywordIsEmpty()
        {
            _storeService.AddCustomer("Марія", "Іванова", "m.ivanova@gmail.com", "0997654321");

            var results = _storeService.SearchCustomers("   ");

            Assert.AreEqual(0, results.Count);
        }
    }
}