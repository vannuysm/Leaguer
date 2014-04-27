namespace Leaguerly.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDivisionAliasIndex : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Divisions", "Alias");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Divisions", new[] { "Alias" });
        }
    }
}
