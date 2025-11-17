namespace StoreApp.Core
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Product> Products { get; set; }

        public Category()
        {
            Products = new List<Product>();
        }

        public Category(int id, string name) : this()
        {
            Id = id;
            Name = name;
        }
    }
}