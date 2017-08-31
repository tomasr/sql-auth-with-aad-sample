using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using BootstrapContext = System.IdentityModel.Tokens.BootstrapContext;

namespace UsingTokenAuthApp.Controllers
{
    public static class ADAuthentication
    {
        static String TenantId = ConfigurationManager.AppSettings["AAD_TENANT_ID"];
        static String Authority = "https://login.windows.net/" + TenantId;
        static String ServicePrincipalName = ConfigurationManager.AppSettings["SERVICE_PRINCIPAL_NAME"];
        static String CertSubjectName = ConfigurationManager.AppSettings["CERT_SUBJECT_NAME"];
        const String SqlResource = "https://database.windows.net/";

       
        public static X509Certificate2 FindCertificate(String subjectName)
        {
            using (var store = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                store.Open(OpenFlags.ReadOnly);
                var results = store.Certificates.Find(X509FindType.FindBySubjectName, CertSubjectName, false);
                if ( results.Count > 0 )
                {
                    return results[0];
                }
            }
            return null;
        }

        public static async Task<String> GetSqlTokenAsync()
        {
            var context = new AuthenticationContext(Authority);
            var certificate = FindCertificate(CertSubjectName);
            if (certificate == null)
                throw new InvalidOperationException("Could not load certificate");

            var credential = new ClientAssertionCertificate($"http://{ServicePrincipalName}", certificate);

            var result = await context.AcquireTokenAsync(SqlResource, credential);
            return result.AccessToken;
        }

        public static async Task<String> GetDelegatedTokenAsync(ClaimsPrincipal user)
        {
            var context = new AuthenticationContext(Authority);
            var certificate = FindCertificate(CertSubjectName);
            if (certificate == null)
                throw new InvalidOperationException("Could not load certificate");

            var id = (ClaimsIdentity)user.Identity;
            var bootstrap = id.BootstrapContext as BootstrapContext;
            var credential = new ClientAssertionCertificate($"http://{ServicePrincipalName}", certificate);

            var userAssertion = new UserAssertion(bootstrap.Token);
            var result = await context.AcquireTokenAsync(SqlResource, credential, userAssertion);
            return result.AccessToken;
        }
    }
}