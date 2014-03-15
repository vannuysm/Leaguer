namespace Leaguerly.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedFieldsEntity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Fields", "LocationId", "dbo.Locations");
            DropIndex("dbo.Fields", new[] { "LocationId" });
            DropTable("dbo.Fields");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Fields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LocationId = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Fields", "LocationId");
            AddForeignKey("dbo.Fields", "LocationId", "dbo.Locations", "Id", cascadeDelete: true);
        }
    }
}
