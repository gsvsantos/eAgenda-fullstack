using eAgenda.Core.Aplicacao.ModuloCompromisso.Commands;
using eAgenda.Core.Dominio.ModuloCompromisso;
using eAgenda.WebAPI.Models.ModuloCompromisso;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebAPI.Controllers;

[ApiController]
[Authorize]
[Route("compromissos")]
public class CompromissoController(IMediator mediator, IRepositorioCompromisso repositorioCompromisso) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CadastrarCompromissoResponse>> Cadastrar(CadastrarCompromissoRequest request)
    {
        CadastrarCompromissoCommand command = new(
            request.Assunto,
            request.DataOcorrencia,
            request.HoraInicio,
            request.HoraTermino,
            request.TipoCompromisso,
            request.Local,
            request.Link,
            request.ContatoId
        );

        Result<CadastrarCompromissoResult> result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors[0]);

        CadastrarCompromissoResponse response = new(result.Value.Id);

        return Created(string.Empty, response);
    }


    [HttpGet]
    public async Task<IActionResult> SelecionarTodos()
    {
        List<Compromisso> compromissosSelecionado = await repositorioCompromisso.SelecionarRegistrosAsync();

        return Ok(compromissosSelecionado);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> SelecionarPorId(Guid id)
    {
        Compromisso? compromissoSelecionado = await repositorioCompromisso.SelecionarRegistroPorIdAsync(id);

        return Ok(compromissoSelecionado);
    }
}
