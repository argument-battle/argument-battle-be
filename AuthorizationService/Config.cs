﻿using IdentityServer4;
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

                    RedirectUris = {""},
                    PostLogoutRedirectUris = {"front-end-sign-in-page-urlas"},
                    AllowedCorsOrigins = {"fronto origin"},

                    AllowedScopes =
                    {
                        "TestApiOne",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email
                    },

                    AccessTokenLifetime = 1,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false
                }
            };
    }
}
