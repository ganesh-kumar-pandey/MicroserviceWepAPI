using Microsoft.AspNetCore.Mvc;
using OrderService.Models;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private static List<Order> orders = new()
        {
            new Order{ Id=101, ProductName="Laptop", Quantity=2},
            new Order{ Id=2, ProductName="Phone", Quantity=5}
        };
        [HttpGet]
        [Route("GetOrders")]
        public IActionResult GetOrders()
        {
            return Ok(orders);
        }
    }
}
