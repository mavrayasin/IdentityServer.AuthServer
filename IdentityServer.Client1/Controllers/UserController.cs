using Duende.IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Globalization;
using System.Security.Claims;

namespace IdentityServer.Client1.Controllers
{
    [Authorize]
    public class UserController(IConfiguration _configuration) : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Name = User.Claims.Where(x => x.Type == "given_name").FirstOrDefault()?.Value;
            return View();
        }
        public async Task LogOut()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }
        public async Task<IActionResult> GetRefreshToken()
        {
            HttpClient httpClient = new();

            var disco = await httpClient.GetDiscoveryDocumentAsync("https://localhost:7009");
            if (disco.IsError)
            {
                //logla
            }

            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            RefreshTokenRequest refreshTokenRequest = new ();
            refreshTokenRequest.ClientId = _configuration["Client1Mvc:ClientId"];
            refreshTokenRequest.ClientSecret = _configuration["Client1Mvc:ClientSecret"];
            refreshTokenRequest.RefreshToken = refreshToken;
            refreshTokenRequest.Address = disco.TokenEndpoint; ///"https://localhost:7009/connect/token";


            var token = await httpClient.RequestRefreshTokenAsync(refreshTokenRequest);
            if (token.IsError)
            {
                // yönlendir bir hata sayfasına
            }
            var tokens = new List<AuthenticationToken>()
            {
                 new AuthenticationToken()
                {
                    Name = OpenIdConnectParameterNames.IdToken,
                    Value = token.IdentityToken
                },
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
            };

            var authenticateResult = await HttpContext.AuthenticateAsync();

            var properties = authenticateResult.Properties;

            //propertileri güncelle
            properties.StoreTokens(tokens);

            await HttpContext.SignInAsync("Cookies", authenticateResult.Principal, properties);



            return RedirectToAction("Index");
        }
    }
}
