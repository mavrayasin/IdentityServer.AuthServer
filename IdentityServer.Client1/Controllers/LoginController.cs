using Duende.IdentityModel.Client;
using IdentityServer.Client1.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using NuGet.Common;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Client1.Controllers
{
    public class LoginController(IConfiguration _configuration) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(_configuration["AuthServerUrl"]);

            if(disco.IsError)
            {
                // Log the error
                return View("Error");
            }
            var token = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = _configuration["ClientResourceOwner:ClientId"],
                ClientSecret = _configuration["ClientResourceOwner:ClientSecret"],
                UserName = loginViewModel.Email,
                Password = loginViewModel.Password
            });
            if (token.IsError)
            {
                ModelState.AddModelError("", "Email veya şifreniz yanlış");
                return View();

            }
            // Set the tokens in the authentication properties

            var userInfo = await client.GetUserInfoAsync(new UserInfoRequest
            {
                Address = disco.UserInfoEndpoint,
                Token = token.AccessToken
            });
            if (userInfo.IsError)
            {
                // Log the error
                return View("Error");
            }
            var authenticationProperties = new AuthenticationProperties();
            authenticationProperties.StoreTokens(new List<AuthenticationToken>
            {
                //     new AuthenticationToken()
                //{
                //         Name = OpenIdConnectParameterNames.IdToken,
                //    Value = token.IdentityToken
                //},
                new AuthenticationToken()
                {
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = token.AccessToken
                },
                new AuthenticationToken()
                {
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = token.RefreshToken
                },
                     new AuthenticationToken()
                {
                         Name = OpenIdConnectParameterNames.ExpiresIn,
                    Value = DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)
                }
            });

           await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(
                    new ClaimsIdentity(
                        userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role"
                        )),
                authenticationProperties);

            return RedirectToAction("Index","User");
        }
    }
}
