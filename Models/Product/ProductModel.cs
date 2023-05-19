namespace ASP121.Models.Product
{
    public class Product
    {
        public string? Name { get; set; }
        public float? Price { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
    }

    public class ProductModel
    {
        public List<Product>? Products { get; set; }
    }
}
