namespace Leaguerly.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedFKs : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Games", "AwayTeam_Id", "dbo.Teams");
            DropForeignKey("dbo.Games", "HomeTeam_Id", "dbo.Teams");
            DropForeignKey("dbo.Games", "Location_Id", "dbo.Locations");

            DropIndex("dbo.Games", new[] { "AwayTeam_Id" });
            DropIndex("dbo.Games", new[] { "HomeTeam_Id" });
            DropIndex("dbo.Games", new[] { "Location_Id" });

            RenameColumn(table: "dbo.Games", name: "AwayTeam_Id", newName: "AwayTeamId");
            RenameColumn(table: "dbo.Games", name: "HomeTeam_Id", newName: "HomeTeamId");
            RenameColumn(table: "dbo.Games", name: "Location_Id", newName: "LocationId");
            RenameColumn(table: "dbo.Games", name: "Result_Id", newName: "ResultId");

            RenameIndex(table: "dbo.Games", name: "IX_Result_Id", newName: "IX_ResultId");

            AlterColumn("dbo.Games", "AwayTeamId", c => c.Int());
            AlterColumn("dbo.Games", "HomeTeamId", c => c.Int());
            AlterColumn("dbo.Games", "LocationId", c => c.Int());

            CreateIndex("dbo.Games", "HomeTeamId");
            CreateIndex("dbo.Games", "AwayTeamId");
            CreateIndex("dbo.Games", "LocationId");

            AddForeignKey("dbo.Games", "AwayTeamId", "dbo.Teams", "Id");
            AddForeignKey("dbo.Games", "HomeTeamId", "dbo.Teams", "Id");
            AddForeignKey("dbo.Games", "LocationId", "dbo.Locations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Games", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Games", "HomeTeamId", "dbo.Teams");
            DropForeignKey("dbo.Games", "AwayTeamId", "dbo.Teams");
            DropIndex("dbo.Games", new[] { "LocationId" });
            DropIndex("dbo.Games", new[] { "AwayTeamId" });
            DropIndex("dbo.Games", new[] { "HomeTeamId" });
            AlterColumn("dbo.Games", "LocationId", c => c.Int());
            AlterColumn("dbo.Games", "HomeTeamId", c => c.Int());
            AlterColumn("dbo.Games", "AwayTeamId", c => c.Int());
            RenameIndex(table: "dbo.Games", name: "IX_ResultId", newName: "IX_Result_Id");
            RenameColumn(table: "dbo.Games", name: "ResultId", newName: "Result_Id");
            RenameColumn(table: "dbo.Games", name: "LocationId", newName: "Location_Id");
            RenameColumn(table: "dbo.Games", name: "HomeTeamId", newName: "HomeTeam_Id");
            RenameColumn(table: "dbo.Games", name: "AwayTeamId", newName: "AwayTeam_Id");
            CreateIndex("dbo.Games", "Location_Id");
            CreateIndex("dbo.Games", "HomeTeam_Id");
            CreateIndex("dbo.Games", "AwayTeam_Id");
            AddForeignKey("dbo.Games", "Location_Id", "dbo.Locations", "Id");
            AddForeignKey("dbo.Games", "HomeTeam_Id", "dbo.Teams", "Id");
            AddForeignKey("dbo.Games", "AwayTeam_Id", "dbo.Teams", "Id");
        }
    }
}
