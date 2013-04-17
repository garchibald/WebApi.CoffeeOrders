using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace CoffeeOrders.Models.Data
{
    public class EFContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public EFContext()
            : base("DefaultConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>().Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }


    public class EFContextInitializer : DropCreateDatabaseIfModelChanges<EFContext>
    {
        protected override void Seed(EFContext context)
        {
            base.Seed(context);
            context.SaveChanges();
        }

        
    }
}