using eAgenda.Core.Aplicacao.Compartilhado;
using eAgenda.Core.Aplicacao.ModuloContato.Commands;
using eAgenda.Core.Dominio.ModuloContato;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Immutable;

namespace eAgenda.Core.Aplicacao.ModuloContato.Handlers;

public class SelecionarContatoPorIdQueryHandler(
    IRepositorioContato repositorioContato,
    ILogger<SelecionarContatoPorIdQueryHandler> logger
) : IRequestHandler<SelecionarContatoPorIdQuery, Result<SelecionarContatoPorIdResult>>
{
    public async Task<Result<SelecionarContatoPorIdResult>> Handle(
        SelecionarContatoPorIdQuery query, CancellationToken cancellationToken)
    {
        try
        {
            Contato? contatoSelecionado = await repositorioContato.SelecionarRegistroPorIdAsync(query.Id);

            if (contatoSelecionado == null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(query.Id));

            SelecionarContatoPorIdResult result = new(
                contatoSelecionado.Id,
                contatoSelecionado.Nome,
                contatoSelecionado.Telefone,
                contatoSelecionado.Email,
                contatoSelecionado.Empresa,
                contatoSelecionado.Cargo,
                contatoSelecionado.Compromissos.Select(r => new DetalhesCompromissoContatoDto(
                        r.Assunto,
                        r.DataOcorrencia,
                        r.HoraInicio,
                        r.HoraTermino
                    )).ToImmutableList()
            );

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
