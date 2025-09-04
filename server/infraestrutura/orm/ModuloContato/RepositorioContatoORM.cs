using eAgenda.Core.Dominio.ModuloContato;
using eAgenda.Infraestrutura.ORM.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Infraestrutura.ORM.ModuloContato;

public class RepositorioContatoORM(AppDbContext contexto)
    : RepositorioBaseORM<Contato>(contexto), IRepositorioContato
{
    public override Task<Contato?> SelecionarRegistroPorIdAsync(Guid idRegistro)
    {
        return registros.Where(c => c.Id.Equals(idRegistro)).Include(c => c.Compromissos).FirstOrDefaultAsync();
    }
}
