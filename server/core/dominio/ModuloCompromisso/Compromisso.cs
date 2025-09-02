using eAgenda.Core.Dominio.Compartilhado;
using eAgenda.Core.Dominio.ModuloContato;
using System.Diagnostics.CodeAnalysis;

namespace eAgenda.Core.Dominio.ModuloCompromisso;

public class Compromisso : EntidadeBase<Compromisso>
{
    public string Assunto { get; set; } = string.Empty;
    public DateTime DataOcorrencia { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraTermino { get; set; }
    public TipoCompromisso TipoCompromisso { get; set; }
    public string Local { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public Contato? Contato { get; set; }

    [ExcludeFromCodeCoverage]
    public Compromisso() { }
    public Compromisso(string assunto, DateTime dataOcorrencia, TimeSpan horaInicio, TimeSpan horaTermino,
        TipoCompromisso tipoCompromisso, string local, string link, Contato? contato
    ) : this()
    {
        Id = Guid.NewGuid();
        Assunto = assunto;
        DataOcorrencia = dataOcorrencia;
        HoraInicio = horaInicio;
        HoraTermino = horaTermino;
        TipoCompromisso = tipoCompromisso;
        Local = local;
        Link = link;
        Contato = contato;
    }

    public void Iniciar()
    {
        HoraInicio = DateTime.UtcNow.TimeOfDay;
    }

    public void Terminar()
    {
        HoraTermino = DateTime.UtcNow.TimeOfDay;
    }

    public override void AtualizarRegistro(Compromisso registroEditado)
    {
        Assunto = registroEditado.Assunto;
        DataOcorrencia = registroEditado.DataOcorrencia;
        HoraInicio = registroEditado.HoraInicio;
        HoraTermino = registroEditado.HoraTermino;
        TipoCompromisso = registroEditado.TipoCompromisso;
        Local = registroEditado.Local;
        Link = registroEditado.Link;
        Contato = registroEditado.Contato;
    }
}
