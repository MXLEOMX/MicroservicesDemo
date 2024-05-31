namespace ProductService.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; } // Propiedad que acepta valores NULL
        public decimal Price { get; set; }
    }
}
