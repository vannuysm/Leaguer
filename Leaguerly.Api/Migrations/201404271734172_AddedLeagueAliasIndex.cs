namespace Leaguerly.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLeagueAliasIndex : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Leagues", "Alias", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Leagues", new[] { "Alias" });
        }
    }
}
