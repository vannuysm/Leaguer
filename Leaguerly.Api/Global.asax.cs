using Leaguerly.Api.Models;
using System.Data.Entity;
using System.Web.Http;

namespace Leaguerly.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start() {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<LeaguerlyDbContext>());

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
