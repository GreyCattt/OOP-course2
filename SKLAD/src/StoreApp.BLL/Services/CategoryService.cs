using StoreApp.Core;

namespace StoreApp.BLL
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public Category AddCategory(string name)
        {
            return _repo.AddCategory(name);
        }

        public void RemoveCategory(int id)
        {
            _repo.RemoveCategory(id);
        }

        public void UpdateCategoryName(int id, string newName)
        {
            _repo.UpdateCategoryName(id, newName);
        }

        public Category GetCategoryById(int id)
        {
            return _repo.GetCategoryById(id);
        }

        public List<Category> GetAllCategories()
        {
            return _repo.GetAllCategories();
        }

        public (Product Product, Category Category) FindProductAndCategory(int productId)
        {
            return _repo.FindProductAndCategory(productId);
        }

        public Product AddProduct(int categoryId, string name, string brand, decimal price, int quantity)
        {
            return _repo.AddProduct(categoryId, name, brand, price, quantity);
        }

        public void RemoveProduct(int productId)
        {
            _repo.RemoveProduct(productId);
        }

        public Product UpdateProduct(int productId, string newName, string newBrand, decimal newPrice, int newQuantity)
        {
            return _repo.UpdateProduct(productId, newName, newBrand, newPrice, newQuantity);
        }

        public void UpdateProductQuantity(int productId, int newQuantity)
        {
            _repo.UpdateProductQuantity(productId, newQuantity);
        }

        public void ChangeProductCategory(int productId, int newCategoryId)
        {
            _repo.ChangeProductCategory(productId, newCategoryId);
        }

        public List<Product> GetAllProducts(string sortBy = "")
        {
            return _repo.GetAllProducts(sortBy);
        }

        public List<Product> SearchProducts(string keyword)
        {
            return _repo.SearchProducts(keyword);
        }
    }
}
