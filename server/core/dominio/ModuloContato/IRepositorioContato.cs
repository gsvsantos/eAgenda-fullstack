using eAgenda.Core.Dominio.Compartilhado;

namespace eAgenda.Core.Dominio.ModuloContato;

public interface IRepositorioContato : IRepositorio<Contato>
{
    public Task<bool> ExisteEmailAsync(string email, CancellationToken ct = default);
    public Task<bool> ExisteTelefoneAsync(string telefone, CancellationToken ct = default);
    public Task<bool> ExisteEmailAsync(string email, Guid idSelecionado, CancellationToken ct = default);
    public Task<bool> ExisteTelefoneAsync(string telefone, Guid idSelecionado, CancellationToken ct = default);
}
