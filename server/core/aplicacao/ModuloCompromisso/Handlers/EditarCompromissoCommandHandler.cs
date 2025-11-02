using AutoMapper;
using eAgenda.Core.Aplicacao.Compartilhado;
using eAgenda.Core.Aplicacao.ModuloCompromisso.Commands;
using eAgenda.Core.Dominio.Compartilhado;
using eAgenda.Core.Dominio.ModuloAutenticacao;
using eAgenda.Core.Dominio.ModuloCompromisso;
using eAgenda.Core.Dominio.ModuloContato;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace eAgenda.Core.Aplicacao.ModuloCompromisso.Handlers;

public class EditarCompromissoCommandHandler(
    IValidator<EditarCompromissoCommand> validator,
    IMapper mapper,
    IRepositorioCompromisso repositorioCompromisso,
    IRepositorioContato repositorioContato,
    ITenantProvider tenantProvider,
    IDistributedCache cache,
    IUnitOfWork unitOfWork,
    ILogger<EditarCompromissoCommandHandler> logger
) : IRequestHandler<EditarCompromissoCommand, Result<EditarCompromissoResult>>
{
    public async Task<Result<EditarCompromissoResult>> Handle(
        EditarCompromissoCommand command, CancellationToken cancellationToken)
    {
        ValidationResult resultValidation = await validator.ValidateAsync(command, cancellationToken);

        if (!resultValidation.IsValid)
        {
            IEnumerable<string> erros = resultValidation.Errors.Select(e => e.ErrorMessage);

            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        List<Compromisso> compromissosExistentes = await repositorioCompromisso.SelecionarRegistrosAsync();

        if (ContemConflito(command, compromissosExistentes))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Há compromisso marcado para esses horário."));
        else if (command.HoraInicio > command.HoraTermino)
            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro("A hora de início está após o horário final."));

        try
        {

            Contato? contatoSelecionado = null;

            if (command.ContatoId.HasValue)
            {
                contatoSelecionado = await repositorioContato.SelecionarRegistroPorIdAsync(command.ContatoId.Value);

                if (contatoSelecionado is null)
                    return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.ContatoId.Value));
            }

            Compromisso novoCompromisso = mapper.Map<Compromisso>(command);

            novoCompromisso.Contato = contatoSelecionado;

            novoCompromisso.UsuarioId = tenantProvider.UsuarioId.GetValueOrDefault();

            await repositorioCompromisso.CadastrarRegistroAsync(novoCompromisso);

            await unitOfWork.CommitAsync();

            // Remove o cache de compromissos do usuário
            string cacheKey = $"contatos:u={tenantProvider.UsuarioId.GetValueOrDefault()}:q=all";

            await cache.RemoveAsync(cacheKey, cancellationToken);

            EditarCompromissoResult result = mapper.Map<EditarCompromissoResult>(novoCompromisso);

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();

            logger.LogError(
                ex,
                "Ocorreu um erro durante o registro de {@Registro}.",
                command
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    private static bool ContemConflito(EditarCompromissoCommand command, List<Compromisso> compromissosExistentes)
    {
        return compromissosExistentes.Any(c =>
                    !c.Id.Equals(command.Id) &&
                    c.DataOcorrencia.Equals(command.DataOcorrencia) &&
                        (
                            (command.HoraInicio >= c.HoraInicio && command.HoraInicio < c.HoraInicio) ||
                            (command.HoraTermino > c.HoraInicio && command.HoraTermino <= c.HoraTermino) ||
                            (command.HoraInicio <= c.HoraInicio && command.HoraTermino >= c.HoraTermino)
                        )
                    );
    }
}
