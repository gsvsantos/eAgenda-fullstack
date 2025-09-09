using eAgenda.Core.Dominio.ModuloContato;
using eAgenda.Infraestrutura.ORM.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Infraestrutura.ORM.ModuloContato;

public class RepositorioContatoORM(AppDbContext contexto)
    : RepositorioBaseORM<Contato>(contexto), IRepositorioContato
{
    public async Task<bool> ExisteEmailAsync(string email, CancellationToken ct = default)
    {
        return await registros.AsNoTracking().AnyAsync(c => c.Email.Equals(email), ct);
    }

    public async Task<bool> ExisteTelefoneAsync(string telefone, CancellationToken ct = default)
    {
        return await registros.AsNoTracking().AnyAsync(c => c.Telefone.Equals(telefone), ct);
    }
    public async Task<bool> ExisteEmailAsync(string email, Guid idSelecionado, CancellationToken ct = default)
    {
        return await registros.AsNoTracking().AnyAsync(c => c.Email == email && c.Id != idSelecionado, ct);
    }

    public async Task<bool> ExisteTelefoneAsync(string telefone, Guid idSelecionado, CancellationToken ct = default)
    {
        return await registros.AsNoTracking().AnyAsync(c => c.Telefone == telefone && c.Id != idSelecionado, ct);
    }

    public override Task<Contato?> SelecionarRegistroPorIdAsync(Guid idRegistro)
    {
        return registros.Where(c => c.Id.Equals(idRegistro)).Include(c => c.Compromissos).FirstOrDefaultAsync();
    }
}
