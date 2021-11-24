using System;
using System.Collections.Generic;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace IdentityService.Configuration.Clients
{
    public class ClientData
    {
        public static IEnumerable<Client> GetClients()
        {
            //Define the development client
            var clientDev = ClientFactory(clientId: "authcodeflowclient_dev", client =>
            {
                client.ClientSecrets = new List<Secret> { new Secret("mysecret".Sha256()) };
                client.AllowOfflineAccess = true;
                client.RedirectUris = new List<string>()
                {
                    "https://localhost:5001/signin-oidc",
                    "https://localhost:5002/signin-oidc",
                    "https://localhost:8001/authcode/callback"
                };

                client.PostLogoutRedirectUris = new List<string>()
                {
                    "https://localhost:5001/signout-callback-oidc"
                };

                client.FrontChannelLogoutUri = "https://localhost:5001/signout-oidc";

                client.AllowedCorsOrigins = new List<string>()
                {
                    "https://localhost:5001"
                };
            });

            //Define the production client
            var clientProd = ClientFactory(clientId: "authcodeflowclient_prod", client =>
            {
                client.ClientSecrets = new List<Secret> { new Secret("mysecret".Sha256()) };
                client.AllowOfflineAccess = true;
                client.RedirectUris = new List<string>()
                {
                    "https://student3-client.secure.nu/signin-oidc"
                };

                client.PostLogoutRedirectUris = new List<string>()
                {
                    "https://student3-client.secure.nu/signout-callback-oidc"
                };

                client.FrontChannelLogoutUri = "https://student3-client.secure.nu/signout-oidc";

                client.AllowedCorsOrigins = new List<string>()
                {
                    "https://student3-client.secure.nu"
                };
            });

            return new List<Client>()
                {
                    clientDev,
                    clientProd
                };
        }


        /// <summary>
        /// Create an instance of a client and populate it with data that should be the same for all clients
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        private static Client ClientFactory(string clientId, Action<Client> clientOptions)
        {
            var baseClient = new Client()
            {
                ClientId = clientId,
                ClientName = "My Client application",
                ClientUri = "https://www.edument.se",
                RequirePkce = true,
                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,

                // When requesting both an id token and access token, should the user claims always
                // be added to the id token instead of requiring the client to use the UserInfo endpoint.
                // Defaults to false.
                AlwaysIncludeUserClaimsInIdToken = false,

                //Specifies whether this client is allowed to receive access tokens via the browser. 
                //This is useful to harden flows that allow multiple response types 
                //(e.g. by disallowing a hybrid flow client that is supposed to  use code id_token to add the token response type and thus leaking the token to the browser.
                AllowAccessTokensViaBrowser = false,

                AllowedScopes =
                    {
                        //Standard scopes
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Phone,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "employee",
                        "payment"
                    },

                AlwaysSendClientClaims = true,
                ClientClaimsPrefix = "client_",
                AccessTokenLifetime = 45,
            };

            clientOptions(baseClient);

            return baseClient;
        }
    }

}
