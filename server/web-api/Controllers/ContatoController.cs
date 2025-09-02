using eAgenda.Core.Aplicacao.ModuloContato.Cadastrar;
using eAgenda.Core.Dominio.ModuloContato;
using eAgenda.WebAPI.Models.ModuloContato.Cadastrar;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ContatoController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IRepositorioContato repositorioContato;
    private readonly ILogger<ContatoController> logger;

    public ContatoController(
        IMediator mediator,
        IRepositorioContato repositorioContato,
        ILogger<ContatoController> logger)
    {
        this.mediator = mediator;
        this.repositorioContato = repositorioContato;
        this.logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar(CadastrarContatoRequest request)
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

    [HttpGet]
    public async Task<IActionResult> SelecionarTodos()
    {
        List<Contato> contatosSelecionados = await repositorioContato.SelecionarRegistrosAsync();

        return Ok(contatosSelecionados);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> SelecionarPorId(Guid id)
    {
        Contato? contatoSelecionado = await repositorioContato.SelecionarRegistroPorIdAsync(id);

        return Ok(contatoSelecionado);
    }
}
