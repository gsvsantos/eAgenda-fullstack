using AutoMapper;
using eAgenda.Core.Aplicacao.Compartilhado;
using eAgenda.Core.Aplicacao.ModuloContato.Commands;
using eAgenda.Core.Dominio.Compartilhado;
using eAgenda.Core.Dominio.ModuloAutenticacao;
using eAgenda.Core.Dominio.ModuloContato;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace eAgenda.Core.Aplicacao.ModuloContato.Handlers;

public class CadastrarContatoCommandHandler(
    IValidator<CadastrarContatoCommand> validator,
    IMapper mapper,
    IRepositorioContato repositorioContato,
    ITenantProvider tenantProvider,
    IDistributedCache cache,
    IUnitOfWork unitOfWork,
    ILogger<CadastrarContatoCommandHandler> logger
) : IRequestHandler<CadastrarContatoCommand, Result<CadastrarContatoResult>>
{
    public async Task<Result<CadastrarContatoResult>> Handle(
        CadastrarContatoCommand command, CancellationToken cancellationToken)
    {
        ValidationResult resultValidation = await validator.ValidateAsync(command, cancellationToken);

        if (!resultValidation.IsValid)
        {
            IEnumerable<string> erros = resultValidation.Errors.Select(e => e.ErrorMessage);

            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        List<Contato> contatosExistestes = await repositorioContato.SelecionarRegistrosAsync();

        bool emailDuplicado = await repositorioContato.ExisteEmailAsync(command.Email, cancellationToken);

        bool telefoneDuplicado = await repositorioContato.ExisteTelefoneAsync(command.Telefone, cancellationToken);

        if (emailDuplicado && telefoneDuplicado)
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("E-mail e Telefone já cadastrados."));

        else if (emailDuplicado)
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("E-mail já cadastrado."));

        else if (telefoneDuplicado)
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Telefone já cadastrado."));

        try
        {
            Contato novoContato = mapper.Map<Contato>(command);

            novoContato.UsuarioId = tenantProvider.UsuarioId.GetValueOrDefault();

            await repositorioContato.CadastrarRegistroAsync(novoContato);

            await unitOfWork.CommitAsync();

            // Remove o cache de contatos do usuário
            string cacheKey = $"contatos:u={tenantProvider.UsuarioId.GetValueOrDefault()}:q=all";

            await cache.RemoveAsync(cacheKey, cancellationToken);

            CadastrarContatoResult result = mapper.Map<CadastrarContatoResult>(novoContato);

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
}
