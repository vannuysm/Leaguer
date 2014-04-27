using System.Data.Entity.Migrations;
using System.Web.Http;

namespace Leaguerly.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start() {
            
            // Make sure we run migrations
            var migrationConfig = new Migrations.Configuration();
            var dbMigrator = new DbMigrator(migrationConfig);
            dbMigrator.Update();

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
