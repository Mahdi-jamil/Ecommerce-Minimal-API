namespace Ecommerce_API.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public void Update(string name)
        {
            Name = name;
        }
    }
}