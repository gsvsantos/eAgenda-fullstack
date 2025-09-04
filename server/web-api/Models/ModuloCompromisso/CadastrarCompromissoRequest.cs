using eAgenda.Core.Dominio.ModuloCompromisso;

namespace eAgenda.WebAPI.Models.ModuloCompromisso;

public record CadastrarCompromissoRequest(
    string Assunto,
    DateTime DataOcorrencia,
    TimeSpan HoraInicio,
    TimeSpan HoraTermino,
    TipoCompromisso TipoCompromisso,
    string Local,
    string Link,
    Guid? ContatoId
);

public record CadastrarCompromissoResponse(Guid Id);