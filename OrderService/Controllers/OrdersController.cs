using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using System.Collections.Generic;
using System.Linq;

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

        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetOrders()
        {
            return Orders;
        }

        [HttpGet("{id}")]
        public ActionResult<Order> GetOrder(int id)
        {
            var order = Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
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
}
