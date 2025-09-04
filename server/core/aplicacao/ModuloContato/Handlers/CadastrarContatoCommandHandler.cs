using AutoMapper;
using eAgenda.Core.Aplicacao.Compartilhado;
using eAgenda.Core.Aplicacao.ModuloContato.Commands;
using eAgenda.Core.Dominio.Compartilhado;
using eAgenda.Core.Dominio.ModuloContato;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eAgenda.Core.Aplicacao.ModuloContato.Handlers;

public class CadastrarContatoCommandHandler(
    IValidator<CadastrarContatoCommand> validator,
    IMapper mapper,
    IRepositorioContato repositorioContato,
    IUnitOfWork unitOfWork,
    ILogger<CadastrarContatoCommandHandler> logger
) : IRequestHandler<CadastrarContatoCommand, Result<CadastrarContatoResult>>
{
    public async Task<Result<CadastrarContatoResult>> Handle(
        CadastrarContatoCommand command, CancellationToken cancellationToken)
    {
        ValidationResult resultValidation = await validator.ValidateAsync(command);

        if (!resultValidation.IsValid)
        {
            IEnumerable<string> erros = resultValidation.Errors.Select(e => e.ErrorMessage);

            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        List<Contato> registros = await repositorioContato.SelecionarRegistrosAsync();

        if (registros.Any(i => i.Nome.Equals(command.Nome)))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Já existe um contato registrado com este nome."));

        try
        {
            Contato novoContato = mapper.Map<Contato>(command);

            await repositorioContato.CadastrarRegistroAsync(novoContato);

            await unitOfWork.CommitAsync();

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
