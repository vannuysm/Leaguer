namespace Leaguerly.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        LastName = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 50),
                        Phone = c.String(),
                        MailingAddress_Address1 = c.String(),
                        MailingAddress_Address2 = c.String(),
                        MailingAddress_City = c.String(),
                        MailingAddress_State = c.String(),
                        MailingAddress_ZipCode = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true);
            
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
                "dbo.Games",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        DivisionId = c.Int(nullable: false),
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
                .ForeignKey("dbo.Divisions", t => t.DivisionId, cascadeDelete: true)
                .Index(t => t.DivisionId)
                .Index(t => t.AwayTeam_Id)
                .Index(t => t.HomeTeam_Id)
                .Index(t => t.Location_Id)
                .Index(t => t.Result_Id);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Division_Id = c.Int(),
                        Manager_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Divisions", t => t.Division_Id)
                .ForeignKey("dbo.Managers", t => t.Manager_Id)
                .Index(t => t.Division_Id)
                .Index(t => t.Manager_Id);
            
            CreateTable(
                "dbo.Managers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Contact_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contacts", t => t.Contact_Id)
                .Index(t => t.Contact_Id);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Contact_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contacts", t => t.Contact_Id)
                .Index(t => t.Contact_Id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .Index(t => t.GameResultId)
                .Index(t => t.Player_Id);
            
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
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Password = c.String(),
                        Contact_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contacts", t => t.Contact_Id)
                .Index(t => t.Contact_Id);
            
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
            DropForeignKey("dbo.Users", "Contact_Id", "dbo.Contacts");
            DropForeignKey("dbo.Divisions", "LeagueId", "dbo.Leagues");
            DropForeignKey("dbo.Games", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.Games", "Result_Id", "dbo.GameResults");
            DropForeignKey("dbo.Goals", "GameResultId", "dbo.GameResults");
            DropForeignKey("dbo.Goals", "Player_Id", "dbo.Players");
            DropForeignKey("dbo.Games", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.Games", "HomeTeam_Id", "dbo.Teams");
            DropForeignKey("dbo.Games", "AwayTeam_Id", "dbo.Teams");
            DropForeignKey("dbo.PlayerTeams", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.PlayerTeams", "Player_Id", "dbo.Players");
            DropForeignKey("dbo.Players", "Contact_Id", "dbo.Contacts");
            DropForeignKey("dbo.Teams", "Manager_Id", "dbo.Managers");
            DropForeignKey("dbo.Managers", "Contact_Id", "dbo.Contacts");
            DropForeignKey("dbo.Teams", "Division_Id", "dbo.Divisions");
            DropIndex("dbo.PlayerTeams", new[] { "Team_Id" });
            DropIndex("dbo.PlayerTeams", new[] { "Player_Id" });
            DropIndex("dbo.Users", new[] { "Contact_Id" });
            DropIndex("dbo.Goals", new[] { "Player_Id" });
            DropIndex("dbo.Goals", new[] { "GameResultId" });
            DropIndex("dbo.Players", new[] { "Contact_Id" });
            DropIndex("dbo.Managers", new[] { "Contact_Id" });
            DropIndex("dbo.Teams", new[] { "Manager_Id" });
            DropIndex("dbo.Teams", new[] { "Division_Id" });
            DropIndex("dbo.Games", new[] { "Result_Id" });
            DropIndex("dbo.Games", new[] { "Location_Id" });
            DropIndex("dbo.Games", new[] { "HomeTeam_Id" });
            DropIndex("dbo.Games", new[] { "AwayTeam_Id" });
            DropIndex("dbo.Games", new[] { "DivisionId" });
            DropIndex("dbo.Divisions", new[] { "LeagueId" });
            DropIndex("dbo.Contacts", new[] { "Email" });
            DropTable("dbo.PlayerTeams");
            DropTable("dbo.Users");
            DropTable("dbo.Leagues");
            DropTable("dbo.Goals");
            DropTable("dbo.GameResults");
            DropTable("dbo.Locations");
            DropTable("dbo.Players");
            DropTable("dbo.Managers");
            DropTable("dbo.Teams");
            DropTable("dbo.Games");
            DropTable("dbo.Divisions");
            DropTable("dbo.Contacts");
        }
    }
}
