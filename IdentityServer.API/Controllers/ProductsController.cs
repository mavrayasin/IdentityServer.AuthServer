using IdentityServer.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [Authorize(Policy = "ReadProduct")]
        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Description = "Description 1", Price = 10.0m, ImageUrl = "https://example.com/image1.jpg", Category = "Category 1", IsAvailable = true },
                new Product { Id = 2, Name = "Product 2", Description = "Description 2", Price = 20.0m, ImageUrl = "https://example.com/image2.jpg", Category = "Category 2", IsAvailable = false },
                new Product { Id = 3, Name = "Product 3", Description = "Description 3", Price = 30.0m, ImageUrl = "https://example.com/image3.jpg", Category = "Category 3", IsAvailable = true }
            };
            return Ok(products);
        }
        [Authorize(Policy = "UpdateOrCreate")]
        public IActionResult UpdateProducts(int id)
        {
            return Ok($"id'si {id} olan product güncellenmiş gibi yapıldı.");
        }
        [Authorize(Policy = "UpdateOrCreate")]
        public IActionResult CreateProducts(Product product)
        {
            return Ok(product);
        }
    }
}
