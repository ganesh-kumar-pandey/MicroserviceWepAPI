using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Models;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private static List<Product> orders = new()
        {
            new Product{ Id=101, Name="Laptop", Price=200},
            new Product{ Id=2, Name="Phone", Price=500}
        };
        [HttpGet]
        [Route("GetProduct")]
        public IActionResult GetProduct()
        {
            return Ok(orders);
        }
    }
}
