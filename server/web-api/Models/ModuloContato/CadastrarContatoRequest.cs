namespace eAgenda.WebAPI.Models.ModuloContato;

public record CadastrarContatoRequest(
    string Nome,
    string Telefone,
    string Email,
    string? Empresa,
    string? Cargo
);

public record CadastrarContatoResponse(Guid Id);
