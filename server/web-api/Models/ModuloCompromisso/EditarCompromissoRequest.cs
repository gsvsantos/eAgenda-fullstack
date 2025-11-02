using eAgenda.Core.Dominio.ModuloCompromisso;

namespace eAgenda.WebAPI.Models.ModuloCompromisso;

public record EditarCompromissoRequest(
    string Assunto,
    DateTime DataOcorrencia,
    TimeSpan HoraInicio,
    TimeSpan HoraTermino,
    TipoCompromisso TipoCompromisso,
    string? Local,
    string? Link,
    Guid? ContatoId = null
);

public record EditarCompromissoResponse(
    string Assunto,
    DateTime DataOcorrencia,
    TimeSpan HoraInicio,
    TimeSpan HoraTermino,
    TipoCompromisso TipoCompromisso,
    string? Local,
    string? Link,
    string? Contato
);