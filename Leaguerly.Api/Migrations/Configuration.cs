using Leaguerly.Api.Models;
using System.Data.Entity.Migrations;

namespace Leaguerly.Api.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<LeaguerlyDbContext>
    {
        public Configuration() {
            AutomaticMigrationsEnabled = false;
        }
    }
}
