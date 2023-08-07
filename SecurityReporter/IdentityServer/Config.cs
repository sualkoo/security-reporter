// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;

namespace IdentityServer
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

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("ApiName")
                {
                    ApiSecrets = {new Secret("secret_for_the_api".Sha256())},
                    Scopes = new List<string> { "ApiName" }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "ConsoleApp_ClientId",
                    ClientSecrets = { new Secret("nieco_nevulgarne".Sha256()) },
                    AccessTokenType = AccessTokenType.Reference,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "ApiName" },
                    //Claims =
                    //{
                    //    new Claim(JwtClaimTypes.WebSite, "...")
                    //}
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
                    Username = "demo",
                    Password = "demo", //.Sha265() removed because it was confusing withouth adding any value, https://github.com/georgekosmidis/IdentityServer4.SetupSample/issues/1
                    //Claims =
                    //{
                    //    new Claim(JwtClaimTypes.Role, "SomeRole")
                    //}
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