using AutoMapper;
using eAgenda.Core.Aplicacao.Compartilhado;
using eAgenda.Core.Aplicacao.ModuloContato.Commands;
using eAgenda.Core.Dominio.ModuloContato;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eAgenda.Core.Aplicacao.ModuloContato.Handlers;

public class SelecionarContatosQueryHandler(
    IMapper mapper,
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
            SelecionarContatosResult result = mapper.Map<SelecionarContatosResult>(contatosExistestes);

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
