using eAgenda.Core.Dominio.ModuloTarefa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eAgenda.Infraestrutura.ORM.ModuloTarefa;

public class MapeadorTarefaORM : IEntityTypeConfiguration<Tarefa>
{
    public void Configure(EntityTypeBuilder<Tarefa> builder)
    {
        builder.Property(t => t.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.HasIndex(t => t.Id)
            .IsUnique();

        builder.Property(t => t.Titulo)
            .IsRequired();

        builder.Property(t => t.Descricao)
            .IsRequired();

        builder.Property(t => t.Prioridade)
            .IsRequired();

        builder.Property(t => t.DataCriacao)
            .IsRequired();

        builder.Property(t => t.DataConclusao)
            .IsRequired(false);

        builder.Property(t => t.Status)
            .IsRequired();

        builder.Property(t => t.PercentualConcluido)
            .IsRequired();

        builder.HasMany(t => t.Itens)
            .WithOne(i => i.Tarefa);
    }
}
