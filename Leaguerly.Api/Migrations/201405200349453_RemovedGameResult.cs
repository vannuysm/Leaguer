namespace Leaguerly.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedGameResult : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Goals", "GameResultId", "dbo.GameResults");
            DropForeignKey("dbo.Games", "ResultId", "dbo.GameResults");
            DropIndex("dbo.Games", new[] { "ResultId" });
            DropIndex("dbo.Goals", new[] { "GameResultId" });
            AddColumn("dbo.Games", "WasForfeited", c => c.Boolean(nullable: false));
            AddColumn("dbo.Games", "ForfeitingTeamId", c => c.Int());
            AddColumn("dbo.Games", "IncludeInStandings", c => c.Boolean(nullable: false));
            AddColumn("dbo.Goals", "GameId", c => c.Int(nullable: false));
            CreateIndex("dbo.Goals", "GameId");
            AddForeignKey("dbo.Goals", "GameId", "dbo.Games", "Id", cascadeDelete: true);
            DropColumn("dbo.Games", "ResultId");
            DropColumn("dbo.Goals", "GameResultId");
            DropTable("dbo.GameResults");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.GameResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameId = c.Int(nullable: false),
                        WasForfeited = c.Boolean(nullable: false),
                        ForfeitingTeamId = c.Int(),
                        IncludeInStandings = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Goals", "GameResultId", c => c.Int(nullable: false));
            AddColumn("dbo.Games", "ResultId", c => c.Int());
            DropForeignKey("dbo.Goals", "GameId", "dbo.Games");
            DropIndex("dbo.Goals", new[] { "GameId" });
            DropColumn("dbo.Goals", "GameId");
            DropColumn("dbo.Games", "IncludeInStandings");
            DropColumn("dbo.Games", "ForfeitingTeamId");
            DropColumn("dbo.Games", "WasForfeited");
            CreateIndex("dbo.Goals", "GameResultId");
            CreateIndex("dbo.Games", "ResultId");
            AddForeignKey("dbo.Games", "ResultId", "dbo.GameResults", "Id");
            AddForeignKey("dbo.Goals", "GameResultId", "dbo.GameResults", "Id", cascadeDelete: true);
        }
    }
}
