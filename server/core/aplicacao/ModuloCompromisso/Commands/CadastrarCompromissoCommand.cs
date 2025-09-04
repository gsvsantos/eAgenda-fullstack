using eAgenda.Core.Dominio.ModuloCompromisso;
using FluentResults;
using MediatR;

namespace eAgenda.Core.Aplicacao.ModuloCompromisso.Commands;

public record CadastrarCompromissoCommand(
    string Assunto,
    DateTime DataOcorrencia,
    TimeSpan HoraInicio,
    TimeSpan HoraTermino,
    TipoCompromisso TipoCompromisso,
    string Local,
    string Link,
    Guid? ContatoId
) : IRequest<Result<CadastrarCompromissoResult>>;

public record CadastrarCompromissoResult(Guid Id);
