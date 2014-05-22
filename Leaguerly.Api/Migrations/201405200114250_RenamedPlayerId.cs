namespace Leaguerly.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedPlayerId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Goals", "Player_Id", "dbo.Players");
            DropIndex("dbo.Goals", new[] { "Player_Id" });
            RenameColumn(table: "dbo.Goals", name: "Player_Id", newName: "PlayerId");
            AlterColumn("dbo.Goals", "PlayerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Goals", "PlayerId");
            AddForeignKey("dbo.Goals", "PlayerId", "dbo.Players", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Goals", "PlayerId", "dbo.Players");
            DropIndex("dbo.Goals", new[] { "PlayerId" });
            AlterColumn("dbo.Goals", "PlayerId", c => c.Int());
            RenameColumn(table: "dbo.Goals", name: "PlayerId", newName: "Player_Id");
            CreateIndex("dbo.Goals", "Player_Id");
            AddForeignKey("dbo.Goals", "Player_Id", "dbo.Players", "Id");
        }
    }
}
