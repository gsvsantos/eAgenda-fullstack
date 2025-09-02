namespace eAgenda.WebAPI.Models.ModuloContato.Cadastrar;

public record CadastrarContatoRequest(
    string Nome,
    string Telefone,
    string Email,
    string? Empresa,
    string? Cargo
);
