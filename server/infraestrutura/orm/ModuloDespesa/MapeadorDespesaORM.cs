using eAgenda.Core.Dominio.ModuloDespesa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eAgenda.Infraestrutura.ORM.ModuloDespesa;

public class MapeadorDespesaORM : IEntityTypeConfiguration<Despesa>
{
    public void Configure(EntityTypeBuilder<Despesa> builder)
    {
        builder.Property(d => d.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.HasIndex(d => d.Id)
            .IsUnique();

        builder.Property(d => d.Titulo)
            .IsRequired();

        builder.Property(d => d.Descricao)
            .IsRequired();

        builder.Property(d => d.DataOcorrencia)
            .IsRequired();

        builder.Property(d => d.Valor)
            .IsRequired();

        builder.Property(d => d.FormaPagamento)
            .IsRequired();

        builder.HasMany(d => d.Categorias)
            .WithMany(c => c.Despesas);
    }
}
