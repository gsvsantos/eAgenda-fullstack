using eAgenda.Core.Dominio.ModuloContato;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eAgenda.Infraestrutura.ORM.ModuloContato;

public class MapeadorContatoORM : IEntityTypeConfiguration<Contato>
{
    public void Configure(EntityTypeBuilder<Contato> builder)
    {
        builder.Property(c => c.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.HasIndex(c => c.Id)
            .IsUnique();

        builder.Property(c => c.Nome)
            .IsRequired();

        builder.Property(c => c.Email)
            .IsRequired();

        builder.Property(c => c.Telefone)
            .IsRequired();

        builder.Property(c => c.Cargo)
            .IsRequired(false);

        builder.Property(c => c.Empresa)
            .IsRequired(false);

        builder.HasMany(c => c.Compromissos)
            .WithOne(c => c.Contato)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
