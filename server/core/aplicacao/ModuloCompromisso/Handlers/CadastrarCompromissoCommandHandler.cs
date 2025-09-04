using AutoMapper;
using eAgenda.Core.Aplicacao.Compartilhado;
using eAgenda.Core.Aplicacao.ModuloCompromisso.Commands;
using eAgenda.Core.Dominio.Compartilhado;
using eAgenda.Core.Dominio.ModuloCompromisso;
using eAgenda.Core.Dominio.ModuloContato;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eAgenda.Core.Aplicacao.ModuloCompromisso.Handlers;
public class CadastrarCompromissoCommandHandler(
    IMapper mapper,
    IRepositorioCompromisso repositorioCompromisso,
    IRepositorioContato repositorioContato,
    IUnitOfWork unitOfWork,
    ILogger<CadastrarCompromissoCommandHandler> logger
) : IRequestHandler<CadastrarCompromissoCommand, Result<CadastrarCompromissoResult>>
{
    public async Task<Result<CadastrarCompromissoResult>> Handle(CadastrarCompromissoCommand command, CancellationToken cancellationToken)
    {
        List<Compromisso> compromissosExistentes = await repositorioCompromisso.SelecionarRegistrosAsync();

        if (ContemConflito(command, compromissosExistentes))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Há compromisso marcado para esses horário."));
        else if (command.HoraInicio > command.HoraTermino)
            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro("A hora de início está após o horário final."));

        Contato? contatoSelecionado = null;

        if (command.ContatoId.HasValue)
        {
            contatoSelecionado = await repositorioContato.SelecionarRegistroPorIdAsync(command.ContatoId.Value);

            if (contatoSelecionado is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.ContatoId.Value));
        }

        try
        {
            Compromisso novoCompromisso = new(
                command.Assunto,
                command.DataOcorrencia,
                command.HoraInicio,
                command.HoraTermino,
                command.TipoCompromisso,
                command.Local,
                command.Link,
                contatoSelecionado
            );

            await repositorioCompromisso.CadastrarRegistroAsync(novoCompromisso);

            await unitOfWork.CommitAsync();

            CadastrarCompromissoResult result = mapper.Map<CadastrarCompromissoResult>(novoCompromisso);

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

    private static bool ContemConflito(CadastrarCompromissoCommand command, List<Compromisso> compromissosExistentes)
    {
        return compromissosExistentes.Any(c =>
                    c.DataOcorrencia.Equals(command.DataOcorrencia) &&
                        (
                            (command.HoraInicio >= c.HoraInicio && command.HoraInicio < c.HoraInicio) ||
                            (command.HoraTermino > c.HoraInicio && command.HoraTermino <= c.HoraTermino) ||
                            (command.HoraInicio <= c.HoraInicio && command.HoraTermino >= c.HoraTermino)
                        )
                    );
    }
}
