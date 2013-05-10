namespace CoffeeOrders.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderReference : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "OrderReference", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "OrderReference");
        }
    }
}
