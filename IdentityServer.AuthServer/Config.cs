using IdentityServer4.Models;

namespace IdentityServer.AuthServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {

            return new List<ApiResource>
            {
                new ApiResource("api1", "My API 1")
                {
                    Scopes = { "api1.read", "api1.write", "api1.update" }
                },
                new ApiResource("api2", "My API 2")
                {
                    Scopes = { "api2.read", "api2.write", "api2.update" }
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
            };
        }
    }
}
