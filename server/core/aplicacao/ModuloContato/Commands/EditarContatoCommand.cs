using FluentResults;
using MediatR;

namespace eAgenda.Core.Aplicacao.ModuloContato.Commands;

public record EditarContatoCommand(
    Guid Id,
    string NovoNome,
    string NovoTelefone,
    string NovoEmail,
    string? NovaEmpresa,
    string? NovoCargo
    ) : IRequest<Result<EditarContatoResult>>;

public record EditarContatoResult(
    string NovoNome,
    string NovoTelefone,
    string NovoEmail,
    string? NovaEmpresa,
    string? NovoCargo
);