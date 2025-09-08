namespace eAgenda.WebAPI.Models.ModuloAutenticacao;

public record RegistrarUsuarioRequest(
    string NomeCompleto,
    string Email,
    string Senha,
    string ConfirmarSenha
);
