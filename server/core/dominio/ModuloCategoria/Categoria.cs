using eAgenda.Core.Dominio.Compartilhado;
using eAgenda.Core.Dominio.ModuloDespesa;
using System.Diagnostics.CodeAnalysis;

namespace eAgenda.Core.Dominio.ModuloCategoria;

public class Categoria : EntidadeBase<Categoria>
{
    public string Titulo { get; set; } = string.Empty;
    public List<Despesa> Despesas { get; set; } = new();

    [ExcludeFromCodeCoverage]
    public Categoria() { }
    public Categoria(string titulo) : this()
    {
        Id = Guid.NewGuid();
        Titulo = titulo;
    }

    public void AderirDespesa(Despesa despesa)
    {
        if (Despesas.Any(c => c.Id.Equals(despesa.Id)))
            return;

        Despesas.Add(despesa);
    }

    public void RemoverDespesa(Despesa despesa)
    {
        if (!Despesas.Any(c => c.Id.Equals(despesa.Id)))
            return;

        Despesas.Remove(despesa);
    }

    public override void AtualizarRegistro(Categoria registroEditado)
    {
        Titulo = registroEditado.Titulo;
    }
}
