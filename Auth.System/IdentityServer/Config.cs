using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client()
                {
                    ClientId = "ClientMvc",
                    ClientSecrets = {new Secret("ClientSecret".ToSha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = 
                    {
                        "ClientMvc"
                    }

                }
            };
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId()
            };

    }
}
