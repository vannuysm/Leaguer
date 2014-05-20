namespace Leaguerly.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProfileEmailNotRequired : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Profiles", new[] { "Email" });
            AlterColumn("dbo.Profiles", "Email", c => c.String(maxLength: 50));
            CreateIndex("dbo.Profiles", "Email");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Profiles", new[] { "Email" });
            AlterColumn("dbo.Profiles", "Email", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.Profiles", "Email", unique: true);
        }
    }
}
