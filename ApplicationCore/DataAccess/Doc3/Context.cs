using ApplicationCore.DataAccess.Doc3.Config;
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
      builder.ApplyConfiguration(new PostReaderConfiguration());
   }
   public DbSet<Post> Posts => Set<Post>();
   public DbSet<Unit> Units => Set<Unit>();
   public DbSet<Reader> Readers => Set<Reader>();
   public DbSet<PostReader> PostReaders => Set<PostReader>();

   public override int SaveChanges() => SaveChangesAsync().GetAwaiter().GetResult();

}
