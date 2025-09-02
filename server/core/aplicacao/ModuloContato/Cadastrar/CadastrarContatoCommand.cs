using FluentResults;
using MediatR;

namespace eAgenda.Core.Aplicacao.ModuloContato.Cadastrar;

public record CadastrarContatoCommand(
    string Nome,
    string Telefone,
    string Email,
    string? Empresa,
    string? Cargo
) : IRequest<Result<CadastrarContatoResult>>;
