using System.Collections.Generic;
using Duende.IdentityServer.Models;

namespace IdentityService.Configuration.Resources
{
    public class ApiResourceData
    {
        public static IEnumerable<ApiResource> Resources()
        {
            return new ApiResource[]
            {
                new ApiResource()
                {
                    Name = "paymentapi",
                    ApiSecrets = new List<Secret>() { new Secret("myapisecret".Sha256()) },

                    Scopes = new List<string> { "payment"},

                    UserClaims = new List<string>
                    {
                        "creditlimit","paymentaccess","admin","name"
                    }
                }

            };
        }
    }
}
