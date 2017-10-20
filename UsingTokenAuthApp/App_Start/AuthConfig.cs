using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Configuration;

namespace UsingTokenAuthApp
{
    public class AuthConfig
    {
        public static void RegisterAuth(IAppBuilder app)
        {
            /*
            var clientId = ConfigurationManager.AppSettings["APP_CLIENT_ID"];
            var tenant = ConfigurationManager.AppSettings["AAD_TENANT_ID"];

            var authority = $"https://login.microsoftonline.com/{tenant}";

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            var options = new OpenIdConnectAuthenticationOptions
            {
                Authority = authority,
                ClientId = clientId,
            };
            options.TokenValidationParameters.SaveSigninToken = true;
            app.UseOpenIdConnectAuthentication(options);
            */
        }
    }
}