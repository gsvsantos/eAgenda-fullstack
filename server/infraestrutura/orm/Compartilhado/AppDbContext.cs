using eAgenda.Core.Dominio.Compartilhado;
using eAgenda.Core.Dominio.ModuloAutenticacao;
using eAgenda.Core.Dominio.ModuloCategoria;
using eAgenda.Core.Dominio.ModuloCompromisso;
using eAgenda.Core.Dominio.ModuloContato;
using eAgenda.Core.Dominio.ModuloDespesa;
using eAgenda.Core.Dominio.ModuloTarefa;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace eAgenda.Infraestrutura.ORM.Compartilhado;

public class AppDbContext(DbContextOptions options) : IdentityDbContext<Usuario, Cargo, Guid>(options), IUnitOfWork
{
    public DbSet<Contato> Contatos { get; set; }
    public DbSet<Compromisso> Compromissos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Despesa> Despesas { get; set; }
    public DbSet<Tarefa> Tarefas { get; set; }
    public DbSet<ItemTarefa> ItensTarefa { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Assembly assembly = typeof(AppDbContext).Assembly;

        modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        base.OnModelCreating(modelBuilder);
    }
    public async Task CommitAsync()
    {
        await SaveChangesAsync();
    }

    public async Task RollbackAsync()
    {
        foreach (EntityEntry entry in ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.State = EntityState.Unchanged;
                    break;

                case EntityState.Modified:
                    entry.State = EntityState.Unchanged;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    break;
            }
        }

        await Task.CompletedTask;
    }
}
