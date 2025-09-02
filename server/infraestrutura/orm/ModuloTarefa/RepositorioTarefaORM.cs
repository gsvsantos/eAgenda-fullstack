using eAgenda.Core.Dominio.ModuloTarefa;
using eAgenda.Infraestrutura.ORM.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Infraestrutura.ORM.ModuloTarefa;

public class RepositorioTarefaORM(AppDbContext contexto)
    : RepositorioBaseORM<Tarefa>(contexto), IRepositorioTarefa
{
    public async Task AtualizarStatusRegistrosAsync()
    {
        List<Tarefa> registros = await SelecionarRegistrosAsync();

        foreach (Tarefa tarefa in registros)
        {
            tarefa.AtualizarStatus(); // refatorar todos metodos pra ser async
        }
    }

    public async Task<List<Tarefa>> SelecionarTarefasPorPrioridadeAsync(string? prioridade)
    {
        NivelPrioridade? prioridadeAtual = prioridade switch
        {
            "Baixa" => NivelPrioridade.Baixa,
            "Media" => NivelPrioridade.Media,
            "Alta" => NivelPrioridade.Alta,
            _ => null
        };

        if (prioridadeAtual == null)
            return [];

        return await registros
            .Where(t => t.Prioridade.Equals(prioridadeAtual))
            .Include(t => t.Itens).ToListAsync();
    }

    public async Task<List<Tarefa>> SelecionarTarefasPorStatusAsync(string? status)
    {
        StatusTarefa? statusAtual = status switch
        {
            "Pendente" => StatusTarefa.Pendente,
            "EmAndamento" => StatusTarefa.EmAndamento,
            "Concluida" => StatusTarefa.Concluida,
            "Cancelada" => StatusTarefa.Cancelada,
            _ => null
        };

        if (statusAtual == null)
            return [];

        return await registros
            .Where(t => t.Status.Equals(statusAtual))
            .Include(t => t.Itens).ToListAsync();
    }
}
