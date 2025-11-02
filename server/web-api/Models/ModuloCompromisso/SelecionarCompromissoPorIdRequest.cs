using eAgenda.Core.Dominio.ModuloCompromisso;

namespace eAgenda.WebAPI.Models.ModuloCompromisso;

public record SelecionarCompromissoPorIdRequest(Guid Id);

public record SelecionarCompromissoPorIdResponse(
    Guid Id,
    string Assunto,
    DateTime DataOcorrencia,
    TimeSpan HoraInicio,
    TimeSpan HoraTermino,
    TipoCompromisso TipoCompromisso,
    string? Local,
    string? Link,
    string? Contato
);
