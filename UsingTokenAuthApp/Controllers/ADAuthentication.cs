using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Threading.Tasks;

namespace UsingTokenAuthApp.Controllers
{
    public static class ADAuthentication
    {
        const String SqlResource = "https://database.windows.net/";

        public static Task<String> GetSqlTokenAsync()
        {
            var provider = new AzureServiceTokenProvider();
            return provider.GetAccessTokenAsync(SqlResource);
        }
    }
}