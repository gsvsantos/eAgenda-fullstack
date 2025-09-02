using eAgenda.Core.Dominio.ModuloCompromisso;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eAgenda.Infraestrutura.ORM.ModuloCompromisso;

public class MapeadorCompromissoORM : IEntityTypeConfiguration<Compromisso>
{
    public void Configure(EntityTypeBuilder<Compromisso> builder)
    {
        builder.Property(c => c.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.HasIndex(c => c.Id)
            .IsUnique();

        builder.Property(c => c.Assunto)
            .IsRequired();

        builder.Property(c => c.DataOcorrencia)
            .IsRequired();

        builder.Property(c => c.HoraInicio)
            .IsRequired();

        builder.Property(c => c.HoraTermino)
            .IsRequired();

        builder.Property(c => c.TipoCompromisso)
            .IsRequired();

        builder.Property(c => c.Local)
            .IsRequired(false);

        builder.Property(c => c.Link)
            .IsRequired(false);

        builder.HasOne(c => c.Contato)
            .WithMany(c => c.Compromissos)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
