
using Duende.IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net.Http;

namespace IdentityServer.Client1.Services
{
    public class ApiResourceHttpClient(IHttpContextAccessor _httpContextAccessor,HttpClient _httpClient) : IApiResourceHttpClient
    {
        
        public async Task<HttpClient> GetHttpClient()
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);


            _httpClient.SetBearerToken(accessToken);
            return _httpClient;
        }
    }
}
