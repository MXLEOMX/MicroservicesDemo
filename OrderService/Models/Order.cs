namespace OrderService.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string? UserName { get; set; } // Nueva propiedad
        public string? ProductName { get; set; } // Nueva propiedad
        public decimal ProductPrice { get; set; } // Nueva propiedad
    }
}
