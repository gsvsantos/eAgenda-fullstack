using AutoMapper;
using eAgenda.Core.Aplicacao.ModuloContato.Commands;
using eAgenda.WebAPI.Models.ModuloContato;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebAPI.Controllers;

[ApiController]
[Authorize]
[Route("contatos")]
public class ContatoController(
    IMapper mapper,
    IMediator mediator
) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CadastrarContatoResponse>> Cadastrar(CadastrarContatoRequest request)
    {
        CadastrarContatoCommand command = mapper.Map<CadastrarContatoCommand>(request);

        Result<CadastrarContatoResult> result = await mediator.Send(command);

        if (result.IsFailed)
        {
            if (result.HasError(e => e.HasMetadataKey("TipoErro")))
            {
                IEnumerable<string> errosDeValidacao = result.Errors
                    .SelectMany(e => e.Reasons.OfType<IError>())
                    .Select(e => e.Message);

                return BadRequest(errosDeValidacao);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        CadastrarContatoResponse response = mapper.Map<CadastrarContatoResponse>(result.Value);

        return Created(string.Empty, response);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<EditarContatoResponse>> Editar(Guid id, EditarContatoRequest request)
    {
        EditarContatoCommand command = mapper.Map<(Guid, EditarContatoRequest), EditarContatoCommand>((id, request));

        Result<EditarContatoResult> result = await mediator.Send(command);

        if (result.IsFailed)
        {
            if (result.HasError(e => e.HasMetadataKey("TipoErro")))
            {
                IEnumerable<string> errosDeValidacao = result.Errors
                    .SelectMany(e => e.Reasons.OfType<IError>())
                    .Select(e => e.Message);

                return BadRequest(errosDeValidacao);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        EditarContatoResponse response = mapper.Map<EditarContatoResponse>(result.Value);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ExcluirContatoResponse>> Excluir(Guid id)
    {
        ExcluirContatoCommand command = mapper.Map<ExcluirContatoCommand>(id);

        Result<ExcluirContatoResult> result = await mediator.Send(command);

        if (result.IsFailed)
        {
            if (result.HasError(e => e.HasMetadataKey("TipoErro")))
            {
                IEnumerable<string> errosDeValidacao = result.Errors
                    .SelectMany(e => e.Reasons.OfType<IError>())
                    .Select(e => e.Message);

                return BadRequest(errosDeValidacao);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<SelecionarContatosResponse>> SelecionarTodos(
        [FromQuery] SelecionarContatosRequest? request, CancellationToken cancellationToken)
    {
        SelecionarContatosQuery query = mapper.Map<SelecionarContatosQuery>(request);

        Result<SelecionarContatosResult> result = await mediator.Send(query, cancellationToken);

        if (result.IsFailed)
        {
            if (result.HasError(e => e.HasMetadataKey("TipoErro")))
            {
                IEnumerable<string> errosDeValidacao = result.Errors
                    .SelectMany(e => e.Reasons.OfType<IError>())
                    .Select(e => e.Message);

                return BadRequest(errosDeValidacao);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        SelecionarContatosResponse response = mapper.Map<SelecionarContatosResponse>(result.Value);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SelecionarContatoPorIdResponse>> SelecionarPorId(Guid id)
    {
        SelecionarContatoPorIdQuery query = mapper.Map<SelecionarContatoPorIdQuery>(id);

        Result<SelecionarContatoPorIdResult> result = await mediator.Send(query);

        if (result.IsFailed)
        {
            if (result.HasError(e => e.HasMetadataKey("TipoErro")))
            {
                IEnumerable<string> errosDeValidacao = result.Errors
                    .SelectMany(e => e.Reasons.OfType<IError>())
                    .Select(e => e.Message);

                return BadRequest(errosDeValidacao);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        SelecionarContatoPorIdResponse response = mapper.Map<SelecionarContatoPorIdResponse>(result.Value);

        return Ok(response);
    }
}
