using eAgenda.Core.Dominio.ModuloCompromisso;
using FluentResults;
using MediatR;

namespace eAgenda.Core.Aplicacao.ModuloCompromisso.Commands;

public record SelecionarCompromissoPorIdQuery(Guid Id) : IRequest<Result<SelecionarCompromissoPorIdResult>>;

public record SelecionarCompromissoPorIdResult(
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
