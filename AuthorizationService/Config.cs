using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace AuthorizationService
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };

        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource>
            {
                new ApiResource("TestApiOne"),
                new ApiResource("TestApiTwo")
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "argument-battle-js",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = { "http://localhost:3061/create" },
                    PostLogoutRedirectUris = {"http://localhost:3061/battle"},
                    AllowedCorsOrigins = {"http://localhost:3061"},

                    AllowedScopes =
                    {
                        "TestApiOne",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email
                    },
               
                    AccessTokenLifetime = 3600,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false
                }
            };
    }
}
