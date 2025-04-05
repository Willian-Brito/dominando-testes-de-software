using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NerdStore.Catalogo.Domain;

namespace NerdStore.Catalogo.Infrastructure;

public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome)
            .IsRequired()
            .HasColumnType("varchar(250)");

        builder.HasData(
            new Categoria("Adesivos", 102),
            new Categoria("Camisetas", 100),
            new Categoria("Canecas", 101)
        );
    }
}