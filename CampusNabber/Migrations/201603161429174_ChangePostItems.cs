namespace CampusNabber.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePostItems : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PostItems", "photo_path_id");
            DropColumn("dbo.PostItemPhotos", "photo_path_id");
            DropColumn("dbo.PostItemPhotos", "actual_photo_path");
            AddColumn("dbo.PostItems", "photo_path_id", c => c.Guid());
            AddColumn("dbo.PostItemPhotos", "num_photos", c => c.Int());
        }

        public override void Down()
        {

        }
    }
}
