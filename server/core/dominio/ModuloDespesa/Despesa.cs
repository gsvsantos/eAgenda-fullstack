using eAgenda.Core.Dominio.Compartilhado;
using eAgenda.Core.Dominio.ModuloCategoria;
using System.Diagnostics.CodeAnalysis;

namespace eAgenda.Core.Dominio.ModuloDespesa;

public class Despesa : EntidadeBase<Despesa>
{
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataOcorrencia { get; set; }
    public decimal Valor { get; set; }
    public MeiosPagamento FormaPagamento { get; set; }
    public List<Categoria> Categorias { get; set; } = new();

    [ExcludeFromCodeCoverage]
    public Despesa() { }
    public Despesa(string titulo, string descricao, DateTime dataOcorrencia, decimal valor, MeiosPagamento formaPagamento) : this()
    {
        Id = Guid.NewGuid();
        Titulo = titulo;
        Descricao = descricao;
        DataOcorrencia = dataOcorrencia;
        Valor = valor;
        FormaPagamento = formaPagamento;
    }

    public void AderirCategoria(Categoria categoria)
    {
        if (Categorias.Contains(categoria))
            return;

        Categorias.Add(categoria);

        categoria.AderirDespesa(this);
    }

    public void RemoverCategoria(Categoria categoria)
    {
        if (!Categorias.Contains(categoria))
            return;

        Categorias.Remove(categoria);

        categoria.RemoverDespesa(this);
    }

    public override void AtualizarRegistro(Despesa registroEditado)
    {
        Titulo = registroEditado.Titulo;
        Descricao = registroEditado.Descricao;
        DataOcorrencia = registroEditado.DataOcorrencia;
        Valor = registroEditado.Valor;
        FormaPagamento = registroEditado.FormaPagamento;
    }
}
