using AutoMapper;
using eAgenda.Core.Aplicacao.ModuloAutenticacao.Commands;
using eAgenda.Core.Dominio.ModuloAutenticacao;
using eAgenda.WebAPI.Models.ModuloAutenticacao;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebAPI.Controllers;

[ApiController]
[Route("auth")]
public class AutenticacaoController(IMediator mediator, IMapper mapper) : Controller
{
    [HttpPost("registrar")]
    public async Task<ActionResult<AccessToken>> Registrar(RegistrarUsuarioRequest request)
    {
        RegistrarUsuarioCommand command = mapper.Map<RegistrarUsuarioCommand>(request);

        Result<AccessToken> result = await mediator.Send(command);

        if (result.IsFailed)
        {
            if (result.HasError(e => e.HasMetadata("TipoErro", m => m.Equals("RequisicaoInvalida"))))
            {
                IEnumerable<string> errosDeValidacao = result.Errors
                    .SelectMany(e => e.Reasons.OfType<IError>())
                    .Select(e => e.Message);

                return BadRequest(errosDeValidacao);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok(result.Value);
    }

    [HttpPost("autenticar")]
    public async Task<ActionResult<AccessToken>> Autenticar(AutenticarUsuarioRequest request)
    {
        AutenticarUsuarioCommand command = mapper.Map<AutenticarUsuarioCommand>(request);

        Result<AccessToken> result = await mediator.Send(command);

        if (result.IsFailed)
        {
            if (result.HasError(e => e.HasMetadata("TipoErro", m => m.Equals("RequisicaoInvalida"))))
            {
                IEnumerable<string> errosDeValidacao = result.Errors
                    .SelectMany(e => e.Reasons.OfType<IError>())
                    .Select(e => e.Message);

                return BadRequest(errosDeValidacao);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok(result.Value);
    }

    [HttpPost("sair")]
    [Authorize]
    public async Task<IActionResult> Sair()
    {
        Result result = await mediator.Send(new SairCommand());

        if (result.IsFailed)
            return StatusCode(StatusCodes.Status500InternalServerError);

        return NoContent();
    }
}
