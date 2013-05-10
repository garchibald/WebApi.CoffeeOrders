namespace CoffeeOrders.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotificationUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "NotificationUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "NotificationUrl");
        }
    }
}
