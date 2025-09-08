namespace eAgenda.Core.Dominio.ModuloAutenticacao;

public record UsuarioAutenticado(
    Guid Id,
    string NomeCompleto,
    string Email
);
