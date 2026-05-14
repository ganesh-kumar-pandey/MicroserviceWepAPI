using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Models;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        static List<Order> orders = new()
        {
            new Order{Id=1,ProductName="Laptop",Quantity=2},
            new Order{Id=2,ProductName="Phone",Quantity=5}
        };

        [HttpGet("GetOrders")]
        public IActionResult GetOrders()
        {
            return Ok(orders);
        }
    }
}