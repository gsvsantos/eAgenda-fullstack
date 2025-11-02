using AutoMapper;
using eAgenda.Core.Aplicacao.Compartilhado;
using eAgenda.Core.Aplicacao.ModuloCompromisso.Commands;
using eAgenda.Core.Dominio.ModuloAutenticacao;
using eAgenda.Core.Dominio.ModuloCompromisso;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace eAgenda.Core.Aplicacao.ModuloCompromisso.Handlers;

public class SelecionarCompromissosQueryHandler(
    IMapper mapper,
    IRepositorioCompromisso repositorioCompromisso,
    ITenantProvider tenantProvider,
    IDistributedCache cache,
    ILogger<SelecionarCompromissosQueryHandler> logger
) : IRequestHandler<SelecionarCompromissosQuery, Result<SelecionarCompromissosResult>>
{
    public async Task<Result<SelecionarCompromissosResult>> Handle(
        SelecionarCompromissosQuery query, CancellationToken cancellationToken)
    {
        try
        {
            string cacheQuery = query.Quantidade.HasValue ? $"q={query.Quantidade.Value}" : "q=all";

            string cacheKey = $"contatos:u={tenantProvider.UsuarioId.GetValueOrDefault()}:{cacheQuery}";

            // Tenta buscar dados no cache
            string? jsonString = await cache.GetStringAsync(cacheKey, cancellationToken);

            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                SelecionarCompromissosResult? resultadoEmCache = JsonSerializer.Deserialize<SelecionarCompromissosResult>(jsonString);

                if (resultadoEmCache is not null)
                    return Result.Ok(resultadoEmCache);
            }

            // Caso não encontre dados no cache, busca direto no banco de dados
            List<Compromisso> registros = query.Quantidade.HasValue ?
                await repositorioCompromisso.SelecionarRegistrosAsync(query.Quantidade.Value) :
                await repositorioCompromisso.SelecionarRegistrosAsync();

            SelecionarCompromissosResult result = mapper.Map<SelecionarCompromissosResult>(registros);

            // Salva os dados atualizados em um novo cache após busca no banco
            string jsonPayload = JsonSerializer.Serialize(result);

            DistributedCacheEntryOptions cacheOptions = new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60) };

            await cache.SetStringAsync(cacheKey, jsonPayload, cacheOptions, cancellationToken);

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a seleção de {@Registros}.",
                query
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
