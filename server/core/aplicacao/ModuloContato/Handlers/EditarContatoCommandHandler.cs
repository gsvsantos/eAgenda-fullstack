using AutoMapper;
using eAgenda.Core.Aplicacao.Compartilhado;
using eAgenda.Core.Aplicacao.ModuloContato.Commands;
using eAgenda.Core.Dominio.Compartilhado;
using eAgenda.Core.Dominio.ModuloContato;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eAgenda.Core.Aplicacao.ModuloContato.Handlers;

public class EditarContatoCommandHandler(
    IMapper mapper,
    IRepositorioContato repositorioContato,
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

        if (contatosExistestes.Any(c => !c.Id.Equals(contatoSelecionado.Id) && c.Email.Equals(command.Email))
            && contatosExistestes.Any(c => !c.Id.Equals(contatoSelecionado.Id) && c.Telefone.Equals(command.Telefone)))
        {
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("E-mail e Telefone já cadastrados."));
        }
        else if (contatosExistestes.Any(c => !c.Id.Equals(contatoSelecionado.Id) && c.Email.Equals(command.Email)))
        {
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("E-mail já cadastrados."));
        }
        else if (contatosExistestes.Any(c => !c.Id.Equals(contatoSelecionado.Id) && c.Telefone.Equals(command.Telefone)))
        {
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Telefone já cadastrados."));
        }

        try
        {
            Contato contatoEditado = mapper.Map<Contato>(command);

            await repositorioContato.EditarRegistroAsync(contatoSelecionado.Id, contatoEditado);

            await unitOfWork.CommitAsync();

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
