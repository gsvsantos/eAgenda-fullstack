namespace eAgenda.WebAPI.Models.ModuloContato;

public record EditarContatoRequest(
    string NovoNome,
    string NovoTelefone,
    string NovoEmail,
    string? NovaEmpresa,
    string? NovoCargo
);

public record EditarContatoResponse(
    string NovoNome,
    string NovoTelefone,
    string NovoEmail,
    string? NovaEmpresa,
    string? NovoCargo
);