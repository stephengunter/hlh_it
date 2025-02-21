using ApplicationCore.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using ApplicationCore.DataAccess.Identity.Config;
using AutoMapper;
using System.Reflection.Emit;

namespace ApplicationCore.DataAccess.Identity;
public class IdentityContext : IdentityDbContext<User, Role, string,
        IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
{
  
   public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
	{
      
   }
   protected override void OnModelCreating(ModelBuilder builder)
   {
      base.OnModelCreating(builder);
      builder.ApplyConfiguration(new UserConfiguration());
      builder.ApplyConfiguration(new UserRoleConfiguration());
      builder.UseOpenIddict();
   }
   public DbSet<Profiles> Profiles => Set<Profiles>();
   public DbSet<App> Apps => Set<App>();

   public override int SaveChanges() => SaveChangesAsync().GetAwaiter().GetResult();

}
