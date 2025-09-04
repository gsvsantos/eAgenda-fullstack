using eAgenda.Core.Aplicacao.ModuloContato.Commands;
using eAgenda.WebAPI.Models.ModuloContato;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebAPI.Controllers;

[ApiController]
[Route("contatos")]
public class ContatoController(IMediator mediator) : ControllerBase
{
    [HttpPost("cadastrar")]
    public async Task<ActionResult<CadastrarContatoResponse>> Cadastrar(CadastrarContatoRequest request)
    {
        CadastrarContatoCommand command = new(
            request.Nome,
            request.Telefone,
            request.Email,
            request.Empresa,
            request.Cargo
        );

        Result<CadastrarContatoResult> result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors[0]);

        CadastrarContatoResponse response = new(result.Value.Id);

        return Created(string.Empty, response);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<EditarContatoResponse>> Editar(Guid id, EditarContatoRequest request)
    {
        EditarContatoCommand command = new(
            id,
            request.Nome,
            request.Telefone,
            request.Email,
            request.Empresa,
            request.Cargo
        );

        Result<EditarContatoResult> result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors[0]);

        EditarContatoResponse response = new(
            result.Value.Nome,
            result.Value.Telefone,
            result.Value.Email,
            result.Value.Empresa,
            result.Value.Cargo
        );

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ExcluirContatoResponse>> Excluir(Guid id)
    {
        ExcluirContatoCommand command = new(
            id
        );

        Result<ExcluirContatoResult> result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors[0]);

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<SelecionarContatosResponse>> SelecionarTodos([FromQuery] SelecionarContatosRequest request)
    {
        SelecionarContatosQuery query = new(request.Quantidade);

        Result<SelecionarContatosResult> result = await mediator.Send(query);

        if (result.IsFailed)
            return BadRequest();

        SelecionarContatosResponse response = new(
             result.Value.Contatos.Count,
             result.Value.Contatos
            );

        return Ok(response);
    }


    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SelecionarContatoPorIdResponse>> SelecionarPorId(Guid id)
    {
        SelecionarContatoPorIdQuery query = new(id);

        Result<SelecionarContatoPorIdResult> result = await mediator.Send(query);

        if (result.IsFailed)
            return BadRequest(result.Errors[0].Message);

        SelecionarContatoPorIdResponse response = new(
            result.Value.Id,
            result.Value.Nome,
            result.Value.Telefone,
            result.Value.Email,
            result.Value.Empresa,
            result.Value.Cargo,
            result.Value.Compromissos
        );

        return Ok(response);
    }
}
