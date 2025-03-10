using ApplicationCore.Consts;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using ApplicationCore.DI;
using ApplicationCore.Settings;
using System.Text.Json.Serialization;
using ApplicationCore.DataAccess.Doc3;
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

   string securityKey = Configuration[$"{SettingsKeys.Auth}:SecurityKey"] ?? "";
   if (String.IsNullOrEmpty(securityKey))
   {
      throw new Exception("Failed Add AddJwtBearer. Empty SecurityKey.");
   }
   builder.Services.AddOpenIddict()
    .AddValidation(options =>
    {
       // Note: the validation handler uses OpenID Connect discovery
       // to retrieve the address of the introspection endpoint.
       options.SetIssuer("https://localhost:7221/");
       options.AddAudiences("doc3-api");

       options.UseIntrospection()
              .SetClientId("doc3-api")
              .SetClientSecret("1378ce6e-69bf-44a1-b63d-3e72bd7d2ced");

       options.UseSystemNetHttp();

       //options.AddEncryptionKey(new SymmetricSecurityKey(
       //  Convert.FromBase64String(securityKey)));
       options.UseAspNetCore();

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