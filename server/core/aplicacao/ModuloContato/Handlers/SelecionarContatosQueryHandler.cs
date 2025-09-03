using eAgenda.Core.Aplicacao.Compartilhado;
using eAgenda.Core.Aplicacao.ModuloContato.Commands;
using eAgenda.Core.Dominio.ModuloContato;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Immutable;

namespace eAgenda.Core.Aplicacao.ModuloContato.Handlers;

public class SelecionarContatosQueryHandler(
    IRepositorioContato repositorioContato,
    ILogger<SelecionarContatosQueryHandler> logger
) : IRequestHandler<SelecionarContatosQuery, Result<SelecionarContatosResult>>
{
    public async Task<Result<SelecionarContatosResult>> Handle(
        SelecionarContatosQuery query, CancellationToken cancellationToken)
    {
        List<Contato> contatosExistestes = query.Quantidade.HasValue ?
            await repositorioContato.SelecionarRegistrosAsync(query.Quantidade.Value) :
            await repositorioContato.SelecionarRegistrosAsync();

        if (contatosExistestes.Count == 0)
            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro("Nenhum contato encontrado."));

        try
        {
            ImmutableList<SelecionarContatosDto> contatosEmDto = contatosExistestes.Select(c => new SelecionarContatosDto(
                c.Id,
                c.Nome,
                c.Telefone,
                c.Email,
                c.Empresa,
                c.Cargo
            )).ToImmutableList();

            SelecionarContatosResult? result = new(contatosEmDto);

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a seleção via {@Query}.",
                query
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
