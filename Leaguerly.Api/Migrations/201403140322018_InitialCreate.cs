namespace Leaguerly.Api.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up() {
            CreateTable("dbo.Divisions", c => new {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 100),
                    Alias = c.String(nullable: false, maxLength: 30, unicode: false),
                    LeagueId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Leagues", t => t.LeagueId, cascadeDelete: true)
                .Index(t => t.LeagueId);
            
            CreateTable("dbo.Teams", c => new {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 100),
                    DivisionId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Divisions", t => t.DivisionId, cascadeDelete: true)
                .Index(t => t.DivisionId);
            
            CreateTable("dbo.Players", c => new {
                    Id = c.Int(nullable: false, identity: true),
                    FirstName = c.String(nullable: false, maxLength: 100),
                    LastName = c.String(nullable: false, maxLength: 100),
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable("dbo.Leagues", c => new {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 100),
                    Alias = c.String(nullable: false, maxLength: 30, unicode: false),
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable("dbo.PlayerTeams", c => new {
                    Player_Id = c.Int(nullable: false),
                    Team_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.Player_Id, t.Team_Id })
                .ForeignKey("dbo.Players", t => t.Player_Id, cascadeDelete: true)
                .ForeignKey("dbo.Teams", t => t.Team_Id, cascadeDelete: true)
                .Index(t => t.Player_Id)
                .Index(t => t.Team_Id);
        }
        
        public override void Down() {
            DropForeignKey("dbo.Divisions", "LeagueId", "dbo.Leagues");
            DropForeignKey("dbo.Teams", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.PlayerTeams", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.PlayerTeams", "Player_Id", "dbo.Players");
            DropIndex("dbo.Divisions", new[] { "LeagueId" });
            DropIndex("dbo.Teams", new[] { "DivisionId" });
            DropIndex("dbo.PlayerTeams", new[] { "Team_Id" });
            DropIndex("dbo.PlayerTeams", new[] { "Player_Id" });
            DropTable("dbo.PlayerTeams");
            DropTable("dbo.Leagues");
            DropTable("dbo.Players");
            DropTable("dbo.Teams");
            DropTable("dbo.Divisions");
        }
    }
}
