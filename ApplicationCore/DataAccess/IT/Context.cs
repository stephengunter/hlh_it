using ApplicationCore.Models.IT;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.DataAccess.IT;
public class ITContext : DbContext
{
  
   public ITContext(DbContextOptions<ITContext> options) : base(options)
	{
      
   }
   protected override void OnModelCreating(ModelBuilder builder)
   {
      base.OnModelCreating(builder);
   }
   public DbSet<Item> Items => Set<Item>();
   public DbSet<ItemTransaction> ItemTransactions => Set<ItemTransaction>();
   public DbSet<ItemBalanceSheet> ItemBalanceSheets => Set<ItemBalanceSheet>();
   public DbSet<Category> Categories => Set<Category>();
   public DbSet<CategoryEntity> CategoryEntities => Set<CategoryEntity>();
   public DbSet<Property> Properties => Set<Property>();

   public override int SaveChanges() => SaveChangesAsync().GetAwaiter().GetResult();

}
