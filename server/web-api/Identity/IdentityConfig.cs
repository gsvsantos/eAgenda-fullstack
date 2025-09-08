using eAgenda.Core.Aplicacao.ModuloAutenticacao;
using eAgenda.Core.Dominio.ModuloAutenticacao;
using eAgenda.Infraestrutura.ORM.Compartilhado;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace eAgenda.WebAPI.Identity;

public static class IdentityConfig
{
    public static void AddIdentityProviders(this IServiceCollection services)
    {
        services.AddScoped<ITokenProvider, JwtProvider>();

        services.AddIdentity<Usuario, Cargo>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();
    }

    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        string? chaveAssinaturaJwt = config["JWT_GENERATION_KEY"];

        if (chaveAssinaturaJwt == null)
            throw new ArgumentException("Não foi possível obter a chave de assinatura de tokens.");

        byte[] chaveEmBytes = Encoding.ASCII.GetBytes(chaveAssinaturaJwt);

        string? audienciaValida = config["JWT_AUDIENCE_DOMAIN"];

        if (audienciaValida == null)
            throw new ArgumentException("Não foi possível obter o domínio da audiência dos tokens.");

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = true;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(chaveEmBytes),
                ValidAudience = audienciaValida,
                ValidIssuer = "eAgenda",
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true
            };
        });
    }
}
