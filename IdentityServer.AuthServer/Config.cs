using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace IdentityServer.AuthServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {

            return new List<ApiResource>
            {
                new ApiResource("resource_api1", "My API 1")
                {
                    Scopes = { "api1.read", "api1.write", "api1.update" },
                    ApiSecrets = { new Secret("secretapi1".Sha256()) },
                },
                new ApiResource("resource_api2", "My API 2")
                {
                    Scopes = { "api2.read", "api2.write", "api2.update" },
                     ApiSecrets = { new Secret("secretapi2".Sha256()) },
                }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("api1.read", "Read access to API 1"),
                new ApiScope("api1.write", "Write access to API 1"),
                new ApiScope("api1.update", "Update access to API 1"),

                new ApiScope("api2.read", "Read access to API 2"),
                new ApiScope("api2.write", "Write access to API 2"),
                new ApiScope("api2.update", "Update access to API 2")
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
                new IdentityResources.Address(),
                new IdentityResource(){ Name = "CountryAndCity", DisplayName = "Country And City", Description = "Kullanıcının ülke ve şehir bilgisi",
                    UserClaims = ["country", "city"] },
                new IdentityResource()
                {
                    Name = "Roles",
                    DisplayName = "Roles",
                    Description = "Kullanıcının rolleri",
                    UserClaims = new List<string> { "role" }
                }
            };
        }
        public static IEnumerable<TestUser> GetUsers()
        {
            return new List<TestUser>()
            {
                new TestUser { SubjectId="1",Username="yasin42",  Password="1234",Claims= new List<Claim>(){
                    new Claim("given_name","Yasin"),
                    new Claim("family_name","ELMAS"),
                   new Claim("country","Türkiye"),
                      new Claim("city","Ankara"),
                      new Claim("role","admin")
                } },
                 new TestUser{ SubjectId="2",Username="yasin23",  Password="1234",Claims= new List<Claim>(){
                new Claim("given_name","Yasin"),
                new Claim("family_name","BORAZAN"),
                  new Claim("country","Türkiye"),
                      new Claim("city","ELAZIĞ"),
                    new Claim("role","customer")
                 } }
            };
        }
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                   new Client()
                {
                    ClientId = "client1",
                    ClientName = "Client 1 app uygulaması",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1.read" }
                },
                   new Client()
                {
                    ClientId = "client2",
                    ClientName = "Client 2 app uygulaması",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1.read","api1.update", "api2.write","api2.update" }
                },
                   new Client()
                {
                    ClientId = "Client1-Mvc",
                    RequirePkce = false,
                    ClientName = "Client 1 app mvc uygulaması",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    RedirectUris =
                    {
                        "https://localhost:7189/signin-oidc"
                    },
                    PostLogoutRedirectUris =
                       {
                             "https://localhost:7189/signout-callback-oidc"
                       },
                     AllowedScopes = {
                       IdentityServerConstants.StandardScopes.Email,
                       IdentityServerConstants.StandardScopes.OpenId,
                       IdentityServerConstants.StandardScopes.Profile,"api1.read",
                       IdentityServerConstants.StandardScopes.OfflineAccess,
                       "CountryAndCity",
                       "Roles",
                       },
                     AccessTokenLifetime = 2*60*60,
                     AllowOfflineAccess = true,
                     RefreshTokenUsage = TokenUsage.ReUse, // or one time
                     RefreshTokenExpiration =TokenExpiration.Absolute,
                     AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds, // 30 gün
                     RequireConsent = false,// consent rıza onayı sonrası giriş 

                },

                     new Client()
                {
                    ClientId = "Client2-Mvc",
                    RequirePkce = false,
                    ClientName = "Client 2 app mvc uygulaması",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    RedirectUris =
                    {
                        "https://localhost:7044/signin-oidc"
                    },
                    PostLogoutRedirectUris =
                       {
                             "https://localhost:7044/signout-callback-oidc"
                       },
                     AllowedScopes = {
                       IdentityServerConstants.StandardScopes.OpenId,
                       IdentityServerConstants.StandardScopes.Profile,"api1.read",
                       IdentityServerConstants.StandardScopes.OfflineAccess,
                       "CountryAndCity",
                       "Roles",
                       },
                     AccessTokenLifetime = 2*60*60,
                     AllowOfflineAccess = true,
                     RefreshTokenUsage = TokenUsage.ReUse, // or one time
                     RefreshTokenExpiration =TokenExpiration.Absolute,
                     AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds, // 30 gün
                     RequireConsent = false,// consent rıza onayı sonrası giriş 

                },
                     new Client()
                     {
                         ClientId = "js-client",
                         RequireClientSecret = false,
                         AllowedGrantTypes = GrantTypes.Code,
                         ClientName = "JavaScript Client(Angular)",
                          AllowedScopes = {
                       IdentityServerConstants.StandardScopes.Email,
                       IdentityServerConstants.StandardScopes.OpenId,
                       IdentityServerConstants.StandardScopes.Profile,"api1.read",
                       IdentityServerConstants.StandardScopes.OfflineAccess,
                       "CountryAndCity",
                       "Roles",
                       },
                          RedirectUris =                           {
                              "http://localhost:4200/callback",
                          },
                          AllowedCorsOrigins =
                          {
                              "http://localhost:4200"
                          },
                          PostLogoutRedirectUris =                 {
                              "http://localhost:4200"
                          },
                     },
                     new Client()
                 {
                     ClientId = "Client1-ResourceOwner-Mvc",

                    ClientName="Client 1 app  mvc uygulaması",
                   ClientSecrets=new[] {new Secret("secret".Sha256())},
                   AllowedGrantTypes= GrantTypes.ResourceOwnerPassword,
                   AllowedScopes = { IdentityServerConstants.StandardScopes.Email, IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, "api1.read",IdentityServerConstants.StandardScopes.OfflineAccess,"CountryAndCity","Roles"},
                   AccessTokenLifetime=2*60*60,
                   AllowOfflineAccess=true,
                   RefreshTokenUsage=TokenUsage.ReUse,
                   RefreshTokenExpiration=TokenExpiration.Absolute,
                   AbsoluteRefreshTokenLifetime=(int) (DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
        },

            };
        }

    }
}
