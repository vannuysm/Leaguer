namespace Leaguerly.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCancelledFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "WasCancelled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "WasCancelled");
        }
    }
}
