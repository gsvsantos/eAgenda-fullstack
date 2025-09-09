using System.Diagnostics.CodeAnalysis;

namespace eAgenda.Core.Dominio.ModuloTarefa;

public class ItemTarefa
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public StatusItemTarefa Status { get; set; }
    public Guid TarefaId { get; set; }
    public Tarefa Tarefa { get; set; } = null!;
    public Guid UsuarioId { get; set; }

    [ExcludeFromCodeCoverage]
    public ItemTarefa() { }
    public ItemTarefa(string titulo, Tarefa tarefa) : this()
    {
        Id = Guid.NewGuid();
        Titulo = titulo;
        Tarefa = tarefa;
        Status = StatusItemTarefa.EmAndamento;
    }

    public void MarcarEmAndamento()
    {
        Status = StatusItemTarefa.EmAndamento;
    }

    public void Concluir()
    {
        Status = StatusItemTarefa.Concluido;
    }

    public void Reabrir()
    {
        if (Status == StatusItemTarefa.Concluido || Status == StatusItemTarefa.Cancelado)
            Status = StatusItemTarefa.EmAndamento;
    }

    public void Cancelar()
    {
        Status = StatusItemTarefa.Cancelado;
    }
}
