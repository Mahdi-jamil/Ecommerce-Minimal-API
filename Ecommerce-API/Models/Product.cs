namespace Ecommerce_API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        // Navigation Property
        public Category Category { get; set; }
        public void Update(string name, string description, decimal price, int categoryId)
        {
            Name = name;
            Description = description;
            Price = price;
            CategoryId = categoryId;
        }
    }
}