using IdentityServer.API2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.API2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetPictures()
        {
            var pictures = new List<Pictures>
            {
                new Pictures {  Id = 1, Name = "resim1", Url = "https://example.com/image1.jpg" },
                new Pictures { Id = 2,  Name = "resim1", Url = "https://example.com/image2.jpg" },
                new Pictures { Id = 3,  Name = "resim1", Url = "https://example.com/image3.jpg" }
            };
            return Ok(pictures);
        }
    }
}
