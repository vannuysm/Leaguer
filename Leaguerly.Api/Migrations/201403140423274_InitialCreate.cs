namespace Leaguerly.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Divisions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Alias = c.String(nullable: false, maxLength: 30, unicode: false),
                        LeagueId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Leagues", t => t.LeagueId, cascadeDelete: true)
                .Index(t => t.LeagueId);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        DivisionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Divisions", t => t.DivisionId, cascadeDelete: true)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        LastName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Fields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LocationId = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.LocationId);
            
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
            
            CreateTable(
                "dbo.Goals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Count = c.Int(nullable: false),
                        GameResultId = c.Int(nullable: false),
                        Player_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Players", t => t.Player_Id)
                .ForeignKey("dbo.GameResults", t => t.GameResultId, cascadeDelete: true)
                .Index(t => t.Player_Id)
                .Index(t => t.GameResultId);
            
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        AwayTeam_Id = c.Int(),
                        HomeTeam_Id = c.Int(),
                        Location_Id = c.Int(),
                        Result_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.AwayTeam_Id)
                .ForeignKey("dbo.Teams", t => t.HomeTeam_Id)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .ForeignKey("dbo.GameResults", t => t.Result_Id)
                .Index(t => t.AwayTeam_Id)
                .Index(t => t.HomeTeam_Id)
                .Index(t => t.Location_Id)
                .Index(t => t.Result_Id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Leagues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Alias = c.String(nullable: false, maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlayerTeams",
                c => new
                    {
                        Player_Id = c.Int(nullable: false),
                        Team_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Player_Id, t.Team_Id })
                .ForeignKey("dbo.Players", t => t.Player_Id, cascadeDelete: true)
                .ForeignKey("dbo.Teams", t => t.Team_Id, cascadeDelete: true)
                .Index(t => t.Player_Id)
                .Index(t => t.Team_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Divisions", "LeagueId", "dbo.Leagues");
            DropForeignKey("dbo.Games", "Result_Id", "dbo.GameResults");
            DropForeignKey("dbo.Games", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.Fields", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Games", "HomeTeam_Id", "dbo.Teams");
            DropForeignKey("dbo.Games", "AwayTeam_Id", "dbo.Teams");
            DropForeignKey("dbo.Goals", "GameResultId", "dbo.GameResults");
            DropForeignKey("dbo.Goals", "Player_Id", "dbo.Players");
            DropForeignKey("dbo.Teams", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.PlayerTeams", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.PlayerTeams", "Player_Id", "dbo.Players");
            DropIndex("dbo.Divisions", new[] { "LeagueId" });
            DropIndex("dbo.Games", new[] { "Result_Id" });
            DropIndex("dbo.Games", new[] { "Location_Id" });
            DropIndex("dbo.Fields", new[] { "LocationId" });
            DropIndex("dbo.Games", new[] { "HomeTeam_Id" });
            DropIndex("dbo.Games", new[] { "AwayTeam_Id" });
            DropIndex("dbo.Goals", new[] { "GameResultId" });
            DropIndex("dbo.Goals", new[] { "Player_Id" });
            DropIndex("dbo.Teams", new[] { "DivisionId" });
            DropIndex("dbo.PlayerTeams", new[] { "Team_Id" });
            DropIndex("dbo.PlayerTeams", new[] { "Player_Id" });
            DropTable("dbo.PlayerTeams");
            DropTable("dbo.Leagues");
            DropTable("dbo.Locations");
            DropTable("dbo.Games");
            DropTable("dbo.Goals");
            DropTable("dbo.GameResults");
            DropTable("dbo.Fields");
            DropTable("dbo.Players");
            DropTable("dbo.Teams");
            DropTable("dbo.Divisions");
        }
    }
}
