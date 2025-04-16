using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityServer.Client1.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Name = User.Claims.Where(x => x.Type == "given_name").FirstOrDefault()?.Value;
            return View();
        }
    }
}
