using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ApplicationCore.Models.Identity;
using ApplicationCore.Consts;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;

namespace ApplicationCore.DataAccess.IT;

public static class SeedData
{

   public static async Task EnsureSeedData(IServiceProvider serviceProvider, ConfigurationManager configuration)
	{
		
		Console.WriteLine("Seeding database...");

		var context = serviceProvider.GetRequiredService<ITContext>();
	   context.Database.EnsureCreated();
      Console.WriteLine("Done seeding database.");
	}
   
}