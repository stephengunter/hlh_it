using ApplicationCore.Consts;

using ApplicationCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using ApplicationCore.Helpers.Doc3.Old;
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
using ApplicationCore.DataAccess.Doc3;
using ApplicationCore.DataAccess.Identity;
using OpenIddict.Validation.AspNetCore;

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

   builder.Services.AddOpenIddict()
    .AddValidation(options =>
    {
       // Note: the validation handler uses OpenID Connect discovery
       // to retrieve the address of the introspection endpoint.
       options.SetIssuer("https://localhost:7221/");
       
    });


   #region Add Configurations

   services.Configure<AppSettings>(Configuration.GetSection(SettingsKeys.App));
   services.Configure<AdminSettings>(Configuration.GetSection(SettingsKeys.Admin));
   services.Configure<AuthSettings>(Configuration.GetSection(SettingsKeys.Auth));
   services.Configure<CompanySettings>(Configuration.GetSection(SettingsKeys.Company));
   #endregion


   string conneString = Configuration.GetConnectionString("Default")!;
   services.AddDbContext<Doc3Context>(options =>
   {
      options.UseSqlServer(conneString);
   });


   services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);


   services.AddCorsPolicy(Configuration);
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