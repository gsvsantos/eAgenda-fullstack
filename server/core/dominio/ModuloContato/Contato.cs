using eAgenda.Core.Dominio.Compartilhado;
using eAgenda.Core.Dominio.ModuloCompromisso;
using System.Diagnostics.CodeAnalysis;

namespace eAgenda.Core.Dominio.ModuloContato;

public class Contato : EntidadeBase<Contato>
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string? Cargo { get; set; } = string.Empty;
    public string? Empresa { get; set; } = string.Empty;
    public List<Compromisso> Compromissos { get; set; } = new();

    [ExcludeFromCodeCoverage]
    public Contato() { }
    public Contato(string nome, string email, string telefone, string? cargo, string? empresa) : this()
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Email = email;
        Telefone = telefone;
        Cargo = cargo;
        Empresa = empresa;
    }

    public void AdicionarCompromisso(Compromisso compromisso)
    {
        if (Compromissos.Any(c => c.Id.Equals(compromisso.Id)))
            return;

        Compromissos.Add(compromisso);
    }
    public void RemoverCompromisso(Compromisso compromisso)
    {
        if (!Compromissos.Any(c => c.Id.Equals(compromisso.Id)))
            return;

        Compromissos.Remove(compromisso);
    }

    public override void AtualizarRegistro(Contato registroEditado)
    {
        Nome = registroEditado.Nome;
        Email = registroEditado.Email;
        Telefone = registroEditado.Telefone;
        Cargo = registroEditado.Cargo;
        Empresa = registroEditado.Empresa;
    }
}
