using eAgenda.Core.Dominio.ModuloCompromisso;
using eAgenda.Infraestrutura.ORM.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Infraestrutura.ORM.ModuloCompromisso;

public class RepositorioCompromissoORM(AppDbContext contexto)
    : RepositorioBaseORM<Compromisso>(contexto), IRepositorioCompromisso
{
    public async Task<bool> ExisteCompromissoPorContatoIdAsync(Guid contatoId, CancellationToken ct = default)
    {
        return await registros.AsNoTracking().AnyAsync(x => x.ContatoId == contatoId, ct);
    }
}
