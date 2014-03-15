namespace Leaguerly.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDivisionToGames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "DivisionId", c => c.Int(nullable: false));
            CreateIndex("dbo.Games", "DivisionId");
            AddForeignKey("dbo.Games", "DivisionId", "dbo.Divisions", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Games", "DivisionId", "dbo.Divisions");
            DropIndex("dbo.Games", new[] { "DivisionId" });
            DropColumn("dbo.Games", "DivisionId");
        }
    }
}
