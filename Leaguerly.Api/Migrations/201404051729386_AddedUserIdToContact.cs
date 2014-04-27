namespace Leaguerly.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserIdToContact : DbMigration
    {
        public override void Up() {
            AddColumn("dbo.Contacts", "UserId", c => c.String(maxLength: 128));
            AddForeignKey("dbo.Contacts", "UserId", "dbo.IdentityUsers", "Id", cascadeDelete: true);
            CreateIndex("dbo.Contacts", "UserId");

            AlterColumn("dbo.Contacts", "FirstName", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Contacts", "LastName", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Divisions", "Name", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Teams", "Name", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Leagues", "Name", c => c.String(nullable: false, maxLength: 128));
        }

        public override void Down() {
            DropIndex("dbo.Contacts", new[] { "UserId" });
            DropForeignKey("dbo.Contacts", "UserId", "dbo.IdentityUsers");

            AlterColumn("dbo.Leagues", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Teams", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Divisions", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Contacts", "LastName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Contacts", "FirstName", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.Contacts", "UserId");
        }
    }
}
