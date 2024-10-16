using System.Security.Cryptography.X509Certificates;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Rsk.Saml;
using Rsk.Saml.Models;
using ServiceProvider = Rsk.Saml.Models.ServiceProvider;

namespace IDP;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource()
        {
            Name="samluser_info",
            DisplayName = "User Information for saml provider",
            UserClaims= new List<string>{
                    "SURID",
                    "UTYPE",
                    "PID",
                    "WDID"
            }
        }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("scope1"),
            new ApiScope("scope2"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // m2m client credentials flow client
            new Client
            {
                ClientId = "m2m.client",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                AllowedScopes = { "scope1" }
            },

            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "interactive",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },
                    
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:44300/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "scope2" }
            },
            new Client
            {
                ClientId = "RSKSaml",
                ClientName = "RSK SAML Example",
                ProtocolType = IdentityServerConstants.ProtocolTypes.Saml2p,
                AllowedScopes = {"openid", "profile", "samluser_info" }
            },
        };
        public static IEnumerable<ServiceProvider> ServiceProviders = new[]
    {
        new ServiceProvider
        {
            EntityId = "RSKSaml",
            NameIdentifierFormat = SamlConstants.NameIdentifierFormats.Transient,
            AssertionConsumerServices = {
                new Service(SamlConstants.BindingTypes.HttpPost, "https://localhost:5002/signin-saml"),
                new Service(SamlConstants.BindingTypes.HttpPost, "https://dev2k16server1.sparinc.com/sp/signin-saml"),
                new Service(SamlConstants.BindingTypes.HttpPost, "https://betaapp5.sparinc.com/sp/signin-saml")
                },
            ClaimsMapping = new Dictionary<string, string>
            {
                { "SURID", "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/SURID"},
                { "UTYPE", "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/UTYPE"},
                { "PID", "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/PID"},
                { "WDID", "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/WDID"},                 
            },
            SigningCertificates = new List<X509Certificate2>()
            {
              new X509Certificate2("Resources/MyTestSamlCert.cer","PYt4cwrMem9FxR6v"),
            }
        }
    };
}
