using eAgenda.Core.Dominio.ModuloCategoria;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eAgenda.Infraestrutura.ORM.ModuloCategoria;

public class MapeadorCategoriaORM : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.Property(c => c.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.HasIndex(c => c.Id)
            .IsUnique();

        builder.Property(c => c.Titulo)
            .IsRequired();

        builder.HasMany(c => c.Despesas)
            .WithMany(d => d.Categorias);
    }
}
