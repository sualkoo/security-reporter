using IdentityServer4.Models;
using IdentityServer4.Test;

namespace webapi
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId()
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "ConsoleApp_ClientId",
                    ClientSecrets = { new Secret("client_secret".Sha256()) },
                    AccessTokenType = AccessTokenType.Reference,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "ApiName" },
                }
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>()
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "admin",
                    Password = "admin",
                }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes =>

            new[]
            {
                new ApiScope("ApiName")
            };

    }
}
