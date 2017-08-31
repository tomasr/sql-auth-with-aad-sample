using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(UsingTokenAuthApp.Startup))]

namespace UsingTokenAuthApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AuthConfig.RegisterAuth(app);
        }
    }
}
