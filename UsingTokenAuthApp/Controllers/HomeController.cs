using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace UsingTokenAuthApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> UsingDelegation()
        {
            var user = this.User as ClaimsPrincipal;
            var token = await ADAuthentication.GetDelegatedTokenAsync(user);

            String cs = ConfigurationManager.ConnectionStrings["SqlDb"].ConnectionString;
            var sqlUser = await GetSqlUserName(cs, token);

            ViewBag.SqlUserName = sqlUser;
            return View("DelegatedContext");
        }

        public async Task<ActionResult> UsingSP()
        {
            String cs = ConfigurationManager.ConnectionStrings["SqlDb"].ConnectionString;

            var token = await ADAuthentication.GetSqlTokenAsync();

            var user = await GetSqlUserName(cs, token);
            ViewBag.SqlUserName = user;
            return View("UserContext");
        }

        private async Task<String> GetSqlUserName(String connectionString, String token)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.AccessToken = token;
                await conn.OpenAsync();
                String text = "SELECT SUSER_SNAME()"; 
                using (var cmd = new SqlCommand(text, conn))
                {
                    var result = await cmd.ExecuteScalarAsync();
                    return result as String;
                }

            }
        }
    }
}