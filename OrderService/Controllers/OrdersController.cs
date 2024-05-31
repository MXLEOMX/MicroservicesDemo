using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private static List<Order> Orders = new List<Order>
        {
            new Order { Id = 1, UserId = 1, ProductId = 1, Quantity = 1 },
            new Order { Id = 2, UserId = 2, ProductId = 2, Quantity = 2 }
        };

        private readonly HttpClient _httpClient;

        public OrdersController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetOrders()
        {
            return Orders;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            // Obtener información del usuario desde UserService
            var userResponse = await _httpClient.GetAsync($"http://localhost:5077/api/users/{order.UserId}");
            if (userResponse.IsSuccessStatusCode)
            {
                var userContent = await userResponse.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<User>(userContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                order.UserName = user?.Name;
            }
            else
            {
                // Log del error si la solicitud falla
                var errorContent = await userResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"Error obteniendo usuario: {userResponse.StatusCode} - {errorContent}");
            }

            // Obtener información del producto desde ProductService
            var productResponse = await _httpClient.GetAsync($"http://localhost:5140/api/products/{order.ProductId}");
            if (productResponse.IsSuccessStatusCode)
            {
                var productContent = await productResponse.Content.ReadAsStringAsync();
                var product = JsonSerializer.Deserialize<Product>(productContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                order.ProductName = product?.Name;
                order.ProductPrice = product?.Price ?? 0;
            }
            else
            {
                // Log del error si la solicitud falla
                var errorContent = await productResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"Error obteniendo producto: {productResponse.StatusCode} - {errorContent}");
            }

            return order;
        }

        [HttpPost]
        public ActionResult<Order> CreateOrder(Order order)
        {
            order.Id = Orders.Max(o => o.Id) + 1;
            Orders.Add(order);
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
    }
}
