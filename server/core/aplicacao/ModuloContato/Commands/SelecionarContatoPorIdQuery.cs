using eAgenda.Core.Dominio.ModuloCompromisso;
using FluentResults;
using MediatR;
using System.Collections.Immutable;

namespace eAgenda.Core.Aplicacao.ModuloContato.Commands;

public record SelecionarContatoPorIdQuery(Guid Id) : IRequest<Result<SelecionarContatoPorIdResult>>;

public record SelecionarContatoPorIdResult(
    Guid Id,
    string Nome,
    string Telefone,
    string? Email,
    string? Empresa,
    string? Cargo,
    ImmutableList<DetalhesCompromissoContatoDto> Compromissos
);

public record DetalhesCompromissoContatoDto(
    string Assunto,
    DateTime DataOcorrencia,
    TimeSpan HoraInicio,
    TimeSpan HoraTermino,
    TipoCompromisso TipoCompromisso,
    string Local,
    string Link
);