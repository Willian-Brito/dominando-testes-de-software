using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NerdStore.Core.Data;
using NerdStore.Core.Messages;
using NerdStore.Vendas.Domain;
using NerdStore.Vendas.Infrastructure.Extensions;

namespace NerdStore.Vendas.Infrastructure;

public class VendasContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediatorHandler;
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<PedidoItem> PedidoItems { get; set; }
    public DbSet<Voucher> Vouchers { get; set; }

    public VendasContext(){ }
    public VendasContext(DbContextOptions<VendasContext> options, IMediator mediatorHandler) : base(options) 
    {
        _mediatorHandler = mediatorHandler;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(100)");

        modelBuilder.Ignore<Event>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VendasContext).Assembly);

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) 
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        modelBuilder.HasSequence<int>("MinhaSequencia").StartsAt(1000).IncrementsBy(1);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured)
        {
            var connectionString = "Server=localhost; Database=NerdStoreDB; User Id=sa; Password=sql@2019; TrustServerCertificate=True;";

            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(connectionString);
        }    

        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

    public async Task<bool> Commit()
    {
        foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("DataCadastro").CurrentValue = DateTime.Now;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property("DataCadastro").IsModified = false;
            }
        }

        var sucesso = await base.SaveChangesAsync() > 0;
        if(sucesso) await _mediatorHandler.PublicarEventos(this);

        return sucesso;
    }
}