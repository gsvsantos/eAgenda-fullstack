using AutoMapper;
using eAgenda.Core.Aplicacao.ModuloCompromisso.Commands;
using eAgenda.WebAPI.Models.ModuloCompromisso;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebAPI.Controllers;

[ApiController]
[Authorize]
[Route("compromissos")]
public class CompromissoController(
    IMapper mapper,
    IMediator mediator
) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CadastrarCompromissoResponse>> Cadastrar(CadastrarCompromissoRequest request)
    {
        CadastrarCompromissoCommand command = mapper.Map<CadastrarCompromissoCommand>(request);

        Result<CadastrarCompromissoResult> result = await mediator.Send(command);

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

        CadastrarCompromissoResponse response = new(result.Value.Id);

        return Created(string.Empty, response);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<EditarCompromissoResponse>> Editar(Guid id, EditarCompromissoRequest request)
    {
        EditarCompromissoCommand command = mapper.Map<(Guid, EditarCompromissoRequest), EditarCompromissoCommand>((id, request));

        Result<EditarCompromissoResult> result = await mediator.Send(command);

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

        EditarCompromissoResponse response = mapper.Map<EditarCompromissoResponse>(result.Value);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ExcluirCompromissoResponse>> Excluir(Guid id)
    {
        ExcluirCompromissoCommand command = mapper.Map<ExcluirCompromissoCommand>(id);

        Result<ExcluirCompromissoResult> result = await mediator.Send(command);

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
    public async Task<ActionResult<SelecionarCompromissosResponse>> SelecionarRegistros(
        [FromQuery] SelecionarCompromissosRequest? request,
        CancellationToken cancellationToken
    )
    {
        SelecionarCompromissosQuery query = mapper.Map<SelecionarCompromissosQuery>(request);

        Result<SelecionarCompromissosResult> result = await mediator.Send(query, cancellationToken);

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

        SelecionarCompromissosResponse response = mapper.Map<SelecionarCompromissosResponse>(result.Value);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SelecionarCompromissoPorIdResponse>> SelecionarRegistroPorId(Guid id)
    {
        SelecionarCompromissoPorIdQuery query = mapper.Map<SelecionarCompromissoPorIdQuery>(id);

        Result<SelecionarCompromissoPorIdResult> result = await mediator.Send(query);

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

        SelecionarCompromissoPorIdResponse response = mapper.Map<SelecionarCompromissoPorIdResponse>(result.Value);

        return Ok(response);
    }
}
