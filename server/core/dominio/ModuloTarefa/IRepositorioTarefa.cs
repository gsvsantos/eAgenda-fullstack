using eAgenda.Core.Dominio.Compartilhado;

namespace eAgenda.Core.Dominio.ModuloTarefa;

public interface IRepositorioTarefa : IRepositorio<Tarefa>
{
    public Task AtualizarStatusRegistrosAsync();
    public Task<List<Tarefa>> SelecionarTarefasPorStatusAsync(string? status);
    public Task<List<Tarefa>> SelecionarTarefasPorPrioridadeAsync(string? prioridade);
}
