using Microsoft.AspNetCore.Identity;

namespace eAgenda.Core.Dominio.ModuloAutenticacao;

public class Usuario : IdentityUser<Guid>
{
    public string FullName { get; set; }

    public Usuario()
    {
        Id = Guid.NewGuid();
        EmailConfirmed = true;
    }
}
