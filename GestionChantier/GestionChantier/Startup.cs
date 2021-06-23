using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Net4MvcClient;
using Owin;
using System.IdentityModel.Tokens.Jwt;


[assembly: OwinStartup(typeof(Startup))]
namespace Net4MvcClient
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies",
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = "GestionChantierBruneau",
                Authority = System.Configuration.ConfigurationManager.AppSettings["AuthentificateurUrl"],
                RedirectUri = System.Configuration.ConfigurationManager.AppSettings["GestionChantierUrl"],
                ResponseType = "id_token token",
                RequireHttpsMetadata = true,
                ClientSecret = "secret3",
                Scope = "openid profile",
                UseTokenLifetime = true,
                TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                },
                SignInAsAuthenticationType = "Cookies",
            });
        }

    }
}