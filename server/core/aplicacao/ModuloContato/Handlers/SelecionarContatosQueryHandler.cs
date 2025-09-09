using AutoMapper;
using eAgenda.Core.Aplicacao.Compartilhado;
using eAgenda.Core.Aplicacao.ModuloContato.Commands;
using eAgenda.Core.Dominio.ModuloAutenticacao;
using eAgenda.Core.Dominio.ModuloContato;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace eAgenda.Core.Aplicacao.ModuloContato.Handlers;

public class SelecionarContatosQueryHandler(
    IMapper mapper,
    IRepositorioContato repositorioContato,
    ITenantProvider tenantProvider,
    IDistributedCache cache,
    ILogger<SelecionarContatosQueryHandler> logger
) : IRequestHandler<SelecionarContatosQuery, Result<SelecionarContatosResult>>
{
    public async Task<Result<SelecionarContatosResult>> Handle(
        SelecionarContatosQuery query, CancellationToken cancellationToken)
    {
        try
        {
            string cacheQuery = query.Quantidade.HasValue ? $"q={query.Quantidade.Value}" : "q=all";

            string cacheKey = $"contatos:u={tenantProvider.UsuarioId.GetValueOrDefault()}:{cacheQuery}";

            // Tenta buscar dados no cache
            string? jsonString = await cache.GetStringAsync(cacheKey, cancellationToken);

            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                SelecionarContatosResult? registrosEmChace = JsonSerializer.Deserialize<SelecionarContatosResult>(jsonString);

                if (registrosEmChace is not null)
                    return Result.Ok(registrosEmChace);
            }

            // Caso não encontre dados no cache, busca direto no banco de dados
            List<Contato> contatosExistestes = query.Quantidade.HasValue ?
                await repositorioContato.SelecionarRegistrosAsync(query.Quantidade.Value) :
                await repositorioContato.SelecionarRegistrosAsync();

            if (contatosExistestes.Count == 0)
                return Result.Fail(ResultadosErro.RequisicaoInvalidaErro("Nenhum contato encontrado."));

            SelecionarContatosResult result = mapper.Map<SelecionarContatosResult>(contatosExistestes);

            // Salva os dados atualizados em um novo cache após busca no banco
            string jsonPayload = JsonSerializer.Serialize(result);

            DistributedCacheEntryOptions cacheOptions = new()
            { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60) };

            await cache.SetStringAsync(cacheKey, jsonPayload, cacheOptions, cancellationToken);

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
