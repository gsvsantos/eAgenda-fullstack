using eAgenda.Core.Dominio.ModuloTarefa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eAgenda.Infraestrutura.ORM.ModuloTarefa;

public class MapeadorItemTarefaORM : IEntityTypeConfiguration<ItemTarefa>
{
    public void Configure(EntityTypeBuilder<ItemTarefa> builder)
    {
        builder.Property(i => i.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.HasIndex(i => i.Id)
            .IsUnique();

        builder.Property(i => i.Titulo)
            .IsRequired();

        builder.Property(t => t.Status)
            .IsRequired();

        builder.HasOne(i => i.Tarefa)
            .WithMany(t => t.Itens)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}