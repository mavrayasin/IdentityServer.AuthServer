using Duende.IdentityModel.Client;
using IdentityServer.Client1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
namespace IdentityServer.Client1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IConfiguration _configuration;

        public ProductsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var products = new List<Product>();
            HttpClient httpClient = new HttpClient();

            //identity serverdan token almak için tüm metodları getiren bir hazır fonksiyon
            var disco = await httpClient.GetDiscoveryDocumentAsync("https://localhost:7009");
            if (disco.IsError)
            {
                ///logla
            }

            ClientCredentialsTokenRequest request = new ClientCredentialsTokenRequest();
            request.ClientId = _configuration["Client:ClientId"];
            request.ClientSecret = _configuration["Client:ClientSecret"];
            request.Address = disco.TokenEndpoint;
            //request.GrantType = "client_credentials";

            var token = await httpClient.RequestClientCredentialsTokenAsync(request);
            if (token.IsError)
            {
                //loglar
            }
            httpClient.SetBearerToken(token.AccessToken);

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
