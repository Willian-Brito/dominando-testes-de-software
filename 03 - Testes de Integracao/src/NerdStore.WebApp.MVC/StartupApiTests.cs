using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NerdStore.Catalogo.Application.AutoMapper;
using NerdStore.Catalogo.Infrastructure;
using NerdStore.Vendas.Infrastructure;
using NerdStore.WebApp.MVC.Data;
using NerdStore.WebApp.MVC.Setup;

namespace NerdStore.WebApp.MVC;

public class StartupApiTests
{
     public IConfiguration Configuration { get; }

     public StartupApiTests(IHostEnvironment hostEnvironment)
     {
         var builder = new ConfigurationBuilder()
             .SetBasePath(hostEnvironment.ContentRootPath)
             .AddJsonFile("appsettings.json", true, true)
             .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
             .AddEnvironmentVariables();

         Configuration = builder.Build();
     }

     // This method gets called by the runtime. Use this method to add services to the container.
     public void ConfigureServices(IServiceCollection services)
     {
         services.AddDatabaseContext(Configuration);

         // JWT
         var appSettingsSection = Configuration.GetSection("AppSettings");
         services.Configure<AppSettings>(appSettingsSection);

         var appSettings = appSettingsSection.Get<AppSettings>();
         var key = Encoding.ASCII.GetBytes(appSettings.Secret);

         services.AddAuthentication(x =>
         {
             x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
             x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         }).AddJwtBearer(x =>
         {
             x.RequireHttpsMetadata = true;
             x.SaveToken = true;
             x.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuerSigningKey = true,
                 IssuerSigningKey = new SymmetricSecurityKey(key),
                 ValidateIssuer = true,
                 ValidateAudience = true,
                 ValidAudience = appSettings.ValidoEm,
                 ValidIssuer = appSettings.Emissor
             };
         });
         
         services.AddAutoMapper(typeof(DomainToViewModelMappingProfile), typeof(ViewModelToDomainMappingProfile));
         services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly));
         services.AddDependencyInjection();
         services.AddHttpContextAccessor();
     }

     // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
     public void Configure(IApplicationBuilder app, IHostEnvironment env)
     {
         if (env.IsDevelopment())
         {
             app.UseDeveloperExceptionPage();
             app.UseDatabaseErrorPage();
         }
         else
         {
             app.UseExceptionHandler("/Home/Error");
             // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
             app.UseHsts();
         }

         app.ApplyMigrations();
         app.UseHttpsRedirection();
         app.UseStaticFiles();
         app.UseRouting();
         app.UseCookiePolicy();
        
         app.UseAuthentication();
         app.UseAuthorization();

         app.UseEndpoints(endpoints =>
         {
             endpoints.MapControllerRoute(
                 name: "default",
                 pattern: "{controller=Vitrine}/{action=Index}/{id?}");

             endpoints.MapRazorPages();
         });
     }

    public class AppSettings
    {
        public string Secret { get; set; }
        public int ExpiracaoHoras { get; set; }
        public string Emissor { get; set; }
        public string ValidoEm { get; set; }
    }
}