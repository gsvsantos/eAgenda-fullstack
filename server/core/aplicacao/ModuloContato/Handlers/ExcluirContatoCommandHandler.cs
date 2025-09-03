using eAgenda.Core.Aplicacao.Compartilhado;
using eAgenda.Core.Aplicacao.ModuloContato.Commands;
using eAgenda.Core.Dominio.Compartilhado;
using eAgenda.Core.Dominio.ModuloCompromisso;
using eAgenda.Core.Dominio.ModuloContato;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eAgenda.Core.Aplicacao.ModuloContato.Handlers;

public class ExcluirContatoCommandHandler
(
    IRepositorioContato repositorioContato,
    IRepositorioCompromisso repositorioCompromisso,
    IUnitOfWork unitOfWork,
    ILogger<ExcluirContatoCommandHandler> logger
) : IRequestHandler<ExcluirContatoCommand, Result<ExcluirContatoResult>>
{
    public async Task<Result<ExcluirContatoResult>> Handle(
        ExcluirContatoCommand command, CancellationToken cancellationToken)
    {
        List<Compromisso> compromissosExistentes = await repositorioCompromisso.SelecionarRegistrosAsync();
        Contato? contatoSelecionado = await repositorioContato.SelecionarRegistroPorIdAsync(command.Id);

        if (contatoSelecionado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

        if (compromissosExistentes.Any(c => c.Contato!.Id.Equals(contatoSelecionado.Id)))
            return Result.Fail(ResultadosErro.ExclusaoBloqueadaErro("Não é possível excluir este contato, pois há compromissos vinculados a ele."));

        try
        {
            await repositorioContato.ExcluirRegistroAsync(contatoSelecionado.Id);

            await unitOfWork.CommitAsync();

            ExcluirContatoResult result = new();

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();

            logger.LogError(
                ex,
                "Ocorreu um erro durante a exclusão de {@Registro}.",
                command
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
