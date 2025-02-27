using ApplicationCore.Consts;

using ApplicationCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using ApplicationCore.DI;
using ApplicationCore.Settings;
using OpenIddict.Server.AspNetCore;
using ApplicationCore.Helpers;
using ApplicationCore.DataAccess.IT;
using System.Text.Json.Serialization;

Log.Logger = new LoggerConfiguration()
   .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
   .Enrich.FromLogContext()
   .WriteTo.Console()
   .CreateBootstrapLogger();

try
{
   Log.Information("Starting web application");
   var builder = WebApplication.CreateBuilder(args);
   var Configuration = builder.Configuration;
   builder.Host.UseSerilog((context, services, configuration) => configuration
         .ReadFrom.Configuration(context.Configuration)
         .ReadFrom.Services(services)
         .Enrich.FromLogContext());

   #region Autofac
   builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
   builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
   {
      builder.RegisterModule<ApplicationCoreModule>();
   });

   #endregion
   var services = builder.Services;

   #region Add Configurations

   services.Configure<AppSettings>(Configuration.GetSection(SettingsKeys.App));
   services.Configure<AdminSettings>(Configuration.GetSection(SettingsKeys.Admin));
   services.Configure<AuthSettings>(Configuration.GetSection(SettingsKeys.Auth));
   services.Configure<CompanySettings>(Configuration.GetSection(SettingsKeys.Company));
   #endregion


   string conneString = Configuration.GetConnectionString("Default")!;
   services.AddDbContext<ITContext>(options =>
   {
      options.UseSqlServer(conneString);
   });

   
   string key = Configuration[$"{SettingsKeys.App}:Key"]!;
   if (String.IsNullOrEmpty(key))
   {
      throw new Exception("app key not been set.");
   }
   
   services.AddScoped<ICryptoService>(provider => new AesGcmCryptoService(key.DeriveKeyFromString()));

   //services.AddCorsPolicy(Configuration);
   services.AddAuthorizationPolicy();
   services.AddDtoMapper();
   services.AddControllers()
      .AddJsonOptions(options =>
      {
         options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
      });
   services.AddSwagger(Configuration);

   var app = builder.Build();
  
   app.UseSerilogRequestLogging();

   if (app.Environment.IsDevelopment())
   {
      if (Configuration[$"{SettingsKeys.Developing}:SeedDatabase"].ToBoolean())
      {
         // Seed Database
         using (var scope = app.Services.CreateScope())
         {
            try
            {
               await SeedData.EnsureSeedData(scope.ServiceProvider, Configuration);
            }
            catch (Exception ex)
            {
               Log.Fatal(ex, "SeedData Error");
            }
         }
      }
      app.UseSwagger();
      app.UseSwaggerUI();
   }
   else
   {

   }

   app.UseHttpsRedirection();

   app.UseStaticFiles(); 
   app.UseRouting();

   app.UseCors("Api");
   
   app.UseAuthentication();
   app.UseAuthorization();

   //app.Use(async (context, next) =>
   //{
   //   if (context.Request.Path == "/")
   //   {
   //      context.Response.ContentType = "text/html";
   //      await context.Response.SendFileAsync("wwwroot/index.html");
   //      return;
   //   }

   //   await next();
   //});

   app.MapControllers();
   app.Run();
}
catch (Exception ex)
{
   Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
   Log.Information("finally");
   Log.CloseAndFlush();
}