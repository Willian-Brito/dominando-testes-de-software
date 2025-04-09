using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NerdStore.Catalogo.Application.AutoMapper;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Catalogo.Infrastructure;
using NerdStore.Catalogo.Infrastructure.Repositories;
using NerdStore.Catalogo.Domain;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Queries;
using NerdStore.Vendas.Domain;
using NerdStore.Vendas.Infrastructure;
using NerdStore.Vendas.Infrastructure.Repository;
using NerdStore.WebApp.MVC.Data;

namespace NerdStore.WebApp.MVC.Setup;

public static class DependencyInjection
{
    public static void AddDependencyInjection(this IServiceCollection services)
    {
        // Notifications
        services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

        // Catalogo
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IProdutoService, ProdutoService>();
        services.AddScoped<IEstoqueService, EstoqueService>();
        services.AddScoped<CatalogoContext>();

            
        // Vendas
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<IPedidoQueries, PedidoQueries>();
        services.AddScoped<VendasContext>();

        services.AddScoped<IRequestHandler<AdicionarItemPedidoCommand, bool>, PedidoCommandHandler>();
        services.AddScoped<IRequestHandler<AtualizarItemPedidoCommand, bool>, PedidoCommandHandler>();
        services.AddScoped<IRequestHandler<RemoverItemPedidoCommand, bool>, PedidoCommandHandler>();
        services.AddScoped<IRequestHandler<AplicarVoucherPedidoCommand, bool>, PedidoCommandHandler>();

        services.AddAutoMapper(typeof(DomainToViewModelMappingProfile), typeof(ViewModelToDomainMappingProfile));
    }
    
    public static void AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        
        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddDefaultIdentity<IdentityUser>()
            .AddDefaultUI()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddDbContext<CatalogoContext>(options =>
            options.UseSqlServer(connectionString, x => x.MigrationsAssembly(typeof(CatalogoContext).Namespace)));

        services.AddDbContext<VendasContext>(options =>
            options.UseSqlServer(connectionString, x => x.MigrationsAssembly(typeof(VendasContext).Namespace)));
    }

    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            void MigrateDb<T>(IServiceScope scope) where T : DbContext
            {
                var context = scope.ServiceProvider.GetService<T>();
                context?.Database.Migrate();
            }

            MigrateDb<ApplicationDbContext>(serviceScope);
            MigrateDb<CatalogoContext>(serviceScope);
            MigrateDb<VendasContext>(serviceScope);
        }
    }
    
    public static IServiceCollection AddInfrastructureJWT(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
             c.SwaggerDoc("v1", new OpenApiInfo
             {
                 Title = "Nerd Store API",
                 Description = "Nerd Store API",
                 Contact = new OpenApiContact { Name = "Nerd Store", Email = "email@nerdstore.io" },
                 License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
             });

             c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
             {
                 Description = "Insira o token JWT desta maneira: Bearer {seu token}",
                 Name = "Authorization",
                 In = ParameterLocation.Header,
                 Type = SecuritySchemeType.ApiKey
             });

             c.AddSecurityRequirement(new OpenApiSecurityRequirement
             {
                 {
                     new OpenApiSecurityScheme
                     {
                         Reference = new OpenApiReference
                         {
                             Type = ReferenceType.SecurityScheme,
                             Id = "Bearer"
                         }
                     },
                     new string[] {}
                 }
             });
        });

        return services;
    }
}