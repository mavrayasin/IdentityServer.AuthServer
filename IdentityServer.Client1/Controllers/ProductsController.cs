using Duende.IdentityModel.Client;
using IdentityServer.Client1.Models;
using IdentityServer.Client1.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Runtime.InteropServices;
namespace IdentityServer.Client1.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IApiResourceHttpClient _apiResourceHttpClient;

        public ProductsController(IConfiguration configuration, IApiResourceHttpClient apiResourceHttpClient)
        {
            _configuration = configuration;
            _apiResourceHttpClient = apiResourceHttpClient;
        }

        public async Task<IActionResult> Index()
        {
            var products = new List<Product>();
            HttpClient httpClient = await _apiResourceHttpClient.GetHttpClient();

            //identity serverdan token almak için tüm metodları getiren bir hazır fonksiyon
            //var disco = await httpClient.GetDiscoveryDocumentAsync("https://localhost:7009");
            //if (disco.IsError)
            //{
            //    ///logla
            //}

            //ClientCredentialsTokenRequest request = new ClientCredentialsTokenRequest();
            //request.ClientId = _configuration["Client:ClientId"];
            //request.ClientSecret = _configuration["Client:ClientSecret"];
            //request.Address = disco.TokenEndpoint;
            ////request.GrantType = "client_credentials";

            //var token = await httpClient.RequestClientCredentialsTokenAsync(request);
            //if (token.IsError)
            //{
            //    //loglar
            //}
            //httpClient.SetBearerToken(token.AccessToken);


            ////service ten çağıralacak şekilde değiştirildi.
            //var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);


            //httpClient.SetBearerToken(accessToken);


            //var response = await httpClient.GetAsync("https://localhost:7150/api/Products/GetProducts");
            var response = await httpClient.GetAsync("https://localhost:7150/api/Products/GetProducts");


            if (response.IsSuccessStatusCode)
            {

                var content = response.Content.ReadAsStringAsync();

                products = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Product>>(content.Result);
            }
            else
            {
                //logla
            }
                return View(products);
        }
    }
}
