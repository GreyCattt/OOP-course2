namespace StoreApp.Core
{
    public interface ICategoryRepository
    {
        Category AddCategory(string name);
        void RemoveCategory(int id);
        void UpdateCategoryName(int id, string newName);
        Category GetCategoryById(int id);
        List<Category> GetAllCategories();

        (Product Product, Category Category) FindProductAndCategory(int productId);
        Product AddProduct(int categoryId, string name, string brand, decimal price, int quantity);
        void RemoveProduct(int productId);
        Product UpdateProduct(int productId, string newName, string newBrand, decimal newPrice, int newQuantity);
        void UpdateProductQuantity(int productId, int newQuantity);
        void ChangeProductCategory(int productId, int newCategoryId);
        List<Product> GetAllProducts(string sortBy = "");
        List<Product> SearchProducts(string keyword);
    }
}
