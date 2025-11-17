namespace StoreApp.Core
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public Product() { } 

        public Product(int id, string name, string brand, decimal price, int quantity)
        {
            Id = id;
            Name = name;
            Brand = brand;
            Price = price;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Назва: \"{Name}\", Бренд: \"{Brand}\", Ціна: {Price:C}, Кількість: {Quantity} шт.";
        }
    }
}