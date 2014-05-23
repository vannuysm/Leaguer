namespace Leaguerly.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPlayerToBooking : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlayerId = c.Int(nullable: false),
                        GameId = c.Int(nullable: false),
                        MisconductCode = c.String(maxLength: 2, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.GameId, cascadeDelete: true)
                .ForeignKey("dbo.Players", t => t.PlayerId, cascadeDelete: true)
                .Index(t => t.PlayerId)
                .Index(t => t.GameId)
                .Index(t => t.MisconductCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "PlayerId", "dbo.Players");
            DropForeignKey("dbo.Bookings", "GameId", "dbo.Games");
            DropIndex("dbo.Bookings", new[] { "MisconductCode" });
            DropIndex("dbo.Bookings", new[] { "GameId" });
            DropIndex("dbo.Bookings", new[] { "PlayerId" });
            DropTable("dbo.Bookings");
        }
    }
}
