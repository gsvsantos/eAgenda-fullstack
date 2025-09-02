using eAgenda.Core.Dominio.Compartilhado;
using System.Diagnostics.CodeAnalysis;

namespace eAgenda.Core.Dominio.ModuloTarefa;

public class Tarefa : EntidadeBase<Tarefa>
{
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public NivelPrioridade Prioridade { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataConclusao { get; set; }
    public StatusTarefa Status { get; set; }
    public double PercentualConcluido { get; set; }
    public List<ItemTarefa> Itens { get; set; } = new();

    private const double PercentualConclusao = 100;
    private const double PercentualPendencia = 0;

    [ExcludeFromCodeCoverage]
    public Tarefa() { }
    public Tarefa(string titulo, string descricao, NivelPrioridade prioridade)
    {
        Id = Guid.NewGuid();
        Titulo = titulo;
        Descricao = descricao;
        Prioridade = prioridade;
        Status = StatusTarefa.Pendente;
        PercentualConcluido = 0;
    }

    public void AdicionarItem(ItemTarefa item)
    {
        Itens.Add(item);
        AtualizarStatus();
    }

    public void RemoverItem(ItemTarefa item)
    {
        Itens.Remove(item);
        AtualizarStatus();
    }

    public ItemTarefa? SelecionarItem(Guid idItem)
    {
        return Itens.FirstOrDefault(i => i.Id.Equals(idItem));
    }

    public void AtualizarStatus()
    {
        PercentualConcluido = CalcularPercentualConcluido();

        if (Itens.Count == 0)
            return;

        if (Status != StatusTarefa.Cancelada)
            Status = ObterStatusParaPercentual(PercentualConcluido);

        if (Status == StatusTarefa.Cancelada && Itens.Count != 0)
            Status = ObterStatusParaPercentual(PercentualConcluido);

        if (Status == StatusTarefa.Concluida)
            DataConclusao = DateTime.UtcNow;

        if (Status == StatusTarefa.Pendente || Status == StatusTarefa.EmAndamento)
            DataConclusao = null;
    }

    public void Concluir()
    {
        foreach (ItemTarefa item in Itens)
            item.Concluir();

        DataConclusao = DateTime.UtcNow;
        AtualizarStatus();
    }

    public void Reabrir()
    {
        foreach (ItemTarefa item in Itens)
            item.Reabrir();

        Status = StatusTarefa.Pendente;
        DataConclusao = null;
    }

    public void Cancelar()
    {
        foreach (ItemTarefa item in Itens)
            item.Cancelar();

        Status = StatusTarefa.Cancelada;
        DataConclusao = null;
    }

    public override void AtualizarRegistro(Tarefa registroEditado)
    {
        Titulo = registroEditado.Titulo;
        Descricao = registroEditado.Descricao;
        Prioridade = registroEditado.Prioridade;
        DataCriacao = registroEditado.DataCriacao;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Tarefa tarefa)
            return false;

        return Id == tarefa.Id &&
               Titulo == tarefa.Titulo &&
               Descricao == tarefa.Descricao &&
               Prioridade == tarefa.Prioridade &&
               DataCriacao == tarefa.DataCriacao &&
               DataConclusao == tarefa.DataConclusao &&
               Status == tarefa.Status &&
               PercentualConcluido == tarefa.PercentualConcluido;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Id, Titulo, Descricao,
            Prioridade, DataCriacao, DataConclusao,
            Status, PercentualConcluido);
    }

    private double CalcularPercentualConcluido()
    {
        if (Itens.Count == 0)
            return 0;

        int quantidadeConcluidos = Itens.Count(item => item.Status == StatusItemTarefa.Concluido);
        return (double)quantidadeConcluidos / Itens.Count * 100;
    }

    private StatusTarefa ObterStatusParaPercentual(double percentualStatus)
    {
        if (Itens.Any(i => i.Status == StatusItemTarefa.Cancelado))
            return StatusTarefa.Cancelada;

        if (percentualStatus >= PercentualConclusao)
            return StatusTarefa.Concluida;

        if (percentualStatus <= PercentualPendencia)
            return StatusTarefa.Pendente;

        return StatusTarefa.EmAndamento;
    }
}
