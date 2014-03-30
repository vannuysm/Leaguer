using System.Linq;
using Leaguerly.Api.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Migrations;

namespace Leaguerly.Api.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<LeaguerlyDbContext>
    {
        private const string DefaultAdminRole = "Admin";
        private const string DefaultAdminUsername = "admin";
        private const string DefaultAdminPassword = "password";

        public Configuration() {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LeaguerlyDbContext context) {
            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            //Create Role Admin if it does not exist
            if (!roleManager.RoleExists(DefaultAdminRole)) {
                roleManager.Create(new IdentityRole(DefaultAdminRole));
            }

            if (!roleManager.FindByName(DefaultAdminRole).Users.Any()) {
                var adminUser = new IdentityUser { UserName = DefaultAdminUsername };
                var adminCreateResult = userManager.Create(adminUser, DefaultAdminPassword);

                if (adminCreateResult.Succeeded) {
                    userManager.AddToRole(adminUser.Id, DefaultAdminRole);
                }
            }

            base.Seed(context);
        }
    }
}
