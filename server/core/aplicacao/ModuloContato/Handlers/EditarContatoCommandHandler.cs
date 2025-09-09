using AutoMapper;
using eAgenda.Core.Aplicacao.Compartilhado;
using eAgenda.Core.Aplicacao.ModuloContato.Commands;
using eAgenda.Core.Dominio.Compartilhado;
using eAgenda.Core.Dominio.ModuloAutenticacao;
using eAgenda.Core.Dominio.ModuloContato;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace eAgenda.Core.Aplicacao.ModuloContato.Handlers;

public class EditarContatoCommandHandler(
    IMapper mapper,
    IRepositorioContato repositorioContato,
    ITenantProvider tenantProvider,
    IDistributedCache cache,
    IUnitOfWork unitOfWork,
    ILogger<EditarContatoCommandHandler> logger
) : IRequestHandler<EditarContatoCommand, Result<EditarContatoResult>>
{
    public async Task<Result<EditarContatoResult>> Handle(
        EditarContatoCommand command, CancellationToken cancellationToken)
    {
        List<Contato> contatosExistestes = await repositorioContato.SelecionarRegistrosAsync();
        Contato? contatoSelecionado = await repositorioContato.SelecionarRegistroPorIdAsync(command.Id);

        if (contatoSelecionado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

        bool emailDuplicado = await repositorioContato.ExisteEmailAsync(
            command.Email, contatoSelecionado.Id, cancellationToken);

        bool telefoneDuplicado = await repositorioContato.ExisteTelefoneAsync(
            command.Telefone, contatoSelecionado.Id, cancellationToken);

        if (emailDuplicado && telefoneDuplicado)
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("E-mail e Telefone já cadastrados."));

        else if (emailDuplicado)
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("E-mail já cadastrado."));

        else if (telefoneDuplicado)
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Telefone já cadastrado."));

        try
        {
            Contato contatoEditado = mapper.Map<Contato>(command);

            await repositorioContato.EditarRegistroAsync(contatoSelecionado.Id, contatoEditado);

            await unitOfWork.CommitAsync();

            // Remove o cache de contatos do usuário
            string cacheKey = $"contatos:u={tenantProvider.UsuarioId.GetValueOrDefault()}:q=all";

            await cache.RemoveAsync(cacheKey, cancellationToken);

            EditarContatoResult result = mapper.Map<EditarContatoResult>(contatoEditado);

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();

            logger.LogError(
                ex,
                "Ocorreu um erro durante a edição de {@Registro}.",
                command
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
