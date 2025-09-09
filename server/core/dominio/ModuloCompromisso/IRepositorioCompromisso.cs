using eAgenda.Core.Dominio.Compartilhado;

namespace eAgenda.Core.Dominio.ModuloCompromisso;

public interface IRepositorioCompromisso : IRepositorio<Compromisso>
{
    public Task<bool> ExisteCompromissoPorContatoIdAsync(Guid contatoId, CancellationToken ct = default);
}
