namespace Leaguerly.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProfiles : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Contacts", newName: "Profiles");
            RenameColumn(table: "dbo.Managers", name: "Contact_Id", newName: "Profile_Id");
            RenameColumn(table: "dbo.Players", name: "Contact_Id", newName: "Profile_Id");
            RenameIndex(table: "dbo.Managers", name: "IX_Contact_Id", newName: "IX_Profile_Id");
            RenameIndex(table: "dbo.Players", name: "IX_Contact_Id", newName: "IX_Profile_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Players", name: "IX_Profile_Id", newName: "IX_Contact_Id");
            RenameIndex(table: "dbo.Managers", name: "IX_Profile_Id", newName: "IX_Contact_Id");
            RenameColumn(table: "dbo.Players", name: "Profile_Id", newName: "Contact_Id");
            RenameColumn(table: "dbo.Managers", name: "Profile_Id", newName: "Contact_Id");
            RenameTable(name: "dbo.Profiles", newName: "Contacts");
        }
    }
}
