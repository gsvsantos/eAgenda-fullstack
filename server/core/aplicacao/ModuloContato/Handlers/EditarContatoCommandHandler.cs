using eAgenda.Core.Aplicacao.Compartilhado;
using eAgenda.Core.Aplicacao.ModuloContato.Commands;
using eAgenda.Core.Dominio.Compartilhado;
using eAgenda.Core.Dominio.ModuloContato;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eAgenda.Core.Aplicacao.ModuloContato.Handlers;

public class EditarContatoCommandHandler(
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

        if (contatosExistestes.Any(c => !c.Id.Equals(contatoSelecionado.Id) && c.Email.Equals(command.NovoEmail))
            && contatosExistestes.Any(c => !c.Id.Equals(contatoSelecionado.Id) && c.Telefone.Equals(command.NovoTelefone)))
        {
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("E-mail e Telefone já cadastrados."));
        }
        else if (contatosExistestes.Any(c => !c.Id.Equals(contatoSelecionado.Id) && c.Email.Equals(command.NovoEmail)))
        {
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("E-mail já cadastrados."));
        }
        else if (contatosExistestes.Any(c => !c.Id.Equals(contatoSelecionado.Id) && c.Telefone.Equals(command.NovoTelefone)))
        {
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Telefone já cadastrados."));
        }

        try
        {
            Contato contatoEditado = new(
                command.NovoNome,
                command.NovoTelefone,
                command.NovoEmail,
                command.NovaEmpresa,
                command.NovoCargo
            );

            await repositorioContato.EditarRegistroAsync(contatoSelecionado.Id, contatoEditado);

            await unitOfWork.CommitAsync();

            EditarContatoResult result = new(
                contatoEditado.Nome,
                contatoEditado.Telefone,
                contatoEditado.Email,
                contatoEditado.Empresa,
                contatoEditado.Cargo
            );

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
