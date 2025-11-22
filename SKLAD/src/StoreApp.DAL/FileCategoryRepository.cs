using StoreApp.Core;
using StoreApp.Core.Exceptions;

namespace StoreApp.DAL
{
    public class FileCategoryRepository : ICategoryRepository
    {
        private readonly IStorageService _storage;

        public FileCategoryRepository(IStorageService storageService)
        {
            _storage = storageService;
        }

        public Category AddCategory(string name)
        {
            var ctx = _storage.LoadContext();
            if (ctx.Categories.Any(c => c.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase)))
                throw new DuplicateItemException($"Категорія з назвою \"{name}\" вже існує.");

            int id = ctx.NextCategoryId++;
            var category = new Category(id, name);
            ctx.Categories.Add(category);
            _storage.SaveContext(ctx);
            return category;
        }

        public void RemoveCategory(int id)
        {
            var ctx = _storage.LoadContext();
            var category = ctx.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
                throw new EntityNotFoundException($"Категорію з ID {id} не знайдено.");
            ctx.Categories.Remove(category);
            _storage.SaveContext(ctx);
        }

        public void UpdateCategoryName(int id, string newName)
        {
            var ctx = _storage.LoadContext();
            var category = ctx.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) throw new EntityNotFoundException($"Категорію з ID {id} не знайдено.");
            category.Name = newName;
            _storage.SaveContext(ctx);
        }

        public Category GetCategoryById(int id)
        {
            var ctx = _storage.LoadContext();
            var category = ctx.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) throw new EntityNotFoundException($"Категорію з ID {id} не знайдено.");
            return category;
        }

        public List<Category> GetAllCategories()
        {
            var ctx = _storage.LoadContext();
            return ctx.Categories.ToList();
        }

        public (Product Product, Category Category) FindProductAndCategory(int productId)
        {
            var ctx = _storage.LoadContext();
            foreach (var category in ctx.Categories)
            {
                var product = category.Products.FirstOrDefault(p => p.Id == productId);
                if (product != null)
                    return (product, category);
            }
            throw new EntityNotFoundException($"Товар з ID {productId} не знайдено в жодній категорії.");
        }

        public Product AddProduct(int categoryId, string name, string brand, decimal price, int quantity)
        {
            var ctx = _storage.LoadContext();
            var category = ctx.Categories.FirstOrDefault(c => c.Id == categoryId);
            if (category == null) throw new EntityNotFoundException($"Категорію з ID {categoryId} не знайдено.");
            int id = ctx.NextProductId++;
            var newProduct = new Product(id, name, brand, price, quantity);
            category.Products.Add(newProduct);
            _storage.SaveContext(ctx);
            return newProduct;
        }

        public void RemoveProduct(int productId)
        {
            var ctx = _storage.LoadContext();
            foreach (var category in ctx.Categories)
            {
                var product = category.Products.FirstOrDefault(p => p.Id == productId);
                if (product != null)
                {
                    category.Products.Remove(product);
                    _storage.SaveContext(ctx);
                    return;
                }
            }
            throw new EntityNotFoundException($"Товар з ID {productId} не знайдено.");
        }

        public Product UpdateProduct(int productId, string newName, string newBrand, decimal newPrice, int newQuantity)
        {
            var ctx = _storage.LoadContext();
            foreach (var category in ctx.Categories)
            {
                var product = category.Products.FirstOrDefault(p => p.Id == productId);
                if (product != null)
                {
                    product.Name = newName;
                    product.Brand = newBrand;
                    product.Price = newPrice;
                    product.Quantity = newQuantity;
                    _storage.SaveContext(ctx);
                    return product;
                }
            }
            throw new EntityNotFoundException($"Товар з ID {productId} не знайдено.");
        }

        public void UpdateProductQuantity(int productId, int newQuantity)
        {
            var ctx = _storage.LoadContext();
            foreach (var category in ctx.Categories)
            {
                var product = category.Products.FirstOrDefault(p => p.Id == productId);
                if (product != null)
                {
                    product.Quantity = newQuantity;
                    _storage.SaveContext(ctx);
                    return;
                }
            }
            throw new EntityNotFoundException($"Товар з ID {productId} не знайдено.");
        }

        public void ChangeProductCategory(int productId, int newCategoryId)
        {
            var ctx = _storage.LoadContext();
            Category? oldCategory = null;
            Product? product = null;
            foreach (var category in ctx.Categories)
            {
                var p = category.Products.FirstOrDefault(pr => pr.Id == productId);
                if (p != null)
                {
                    oldCategory = category;
                    product = p;
                    break;
                }
            }
            if (product == null) throw new EntityNotFoundException($"Товар з ID {productId} не знайдено.");
            var newCategory = ctx.Categories.FirstOrDefault(c => c.Id == newCategoryId);
            if (newCategory == null) throw new EntityNotFoundException($"Категорію з ID {newCategoryId} не знайдено.");
            if (oldCategory != null && oldCategory.Id == newCategory.Id) throw new ValidationException("Товар вже знаходиться в цій категорії.");
            oldCategory!.Products.Remove(product!);
            newCategory!.Products.Add(product!);
            _storage.SaveContext(ctx);
        }

        public List<Product> GetAllProducts(string sortBy = "")
        {
            var ctx = _storage.LoadContext();
            var allProducts = ctx.Categories.SelectMany(c => c.Products);
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

            string lower = keyword.ToLower();
            return GetAllProducts()
                .Where(p => p.Name.ToLower().Contains(lower) || p.Brand.ToLower().Contains(lower))
                .ToList();
        }
    }
}
