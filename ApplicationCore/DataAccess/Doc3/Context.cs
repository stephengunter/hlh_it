using ApplicationCore.Models.Doc3;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.DataAccess.Doc3;
public class Doc3Context : DbContext
{
  
   public Doc3Context(DbContextOptions<Doc3Context> options) : base(options)
	{
      
   }
   protected override void OnModelCreating(ModelBuilder builder)
   {
      base.OnModelCreating(builder);
   }
   public DbSet<Post> Posts => Set<Post>();
   public DbSet<Reader> Readers => Set<Reader>();

   public override int SaveChanges() => SaveChangesAsync().GetAwaiter().GetResult();

}
