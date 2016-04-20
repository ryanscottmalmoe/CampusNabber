namespace CampusNabber.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SchoolId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "school_id", c => c.Guid(nullable: false));
        }

        public override void Down()
        {
        }
    }
}
