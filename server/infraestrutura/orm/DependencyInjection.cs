using eAgenda.Core.Dominio.Compartilhado;
using eAgenda.Core.Dominio.ModuloCategoria;
using eAgenda.Core.Dominio.ModuloCompromisso;
using eAgenda.Core.Dominio.ModuloContato;
using eAgenda.Core.Dominio.ModuloDespesa;
using eAgenda.Core.Dominio.ModuloTarefa;
using eAgenda.Infraestrutura.ORM.Compartilhado;
using eAgenda.Infraestrutura.ORM.ModuloCategoria;
using eAgenda.Infraestrutura.ORM.ModuloCompromisso;
using eAgenda.Infraestrutura.ORM.ModuloContato;
using eAgenda.Infraestrutura.ORM.ModuloDespesa;
using eAgenda.Infraestrutura.ORM.ModuloTarefa;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eAgenda.Infraestrutura.ORM;

public static class DependencyInjection
{
    public static IServiceCollection AddCamadaInfraestruturaOrm(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRepositorioContato, RepositorioContatoORM>();
        services.AddScoped<IRepositorioCompromisso, RepositorioCompromissoORM>();
        services.AddScoped<IRepositorioCategoria, RepositorioCategoriaORM>();
        services.AddScoped<IRepositorioDespesa, RepositorioDespesaORM>();
        services.AddScoped<IRepositorioTarefa, RepositorioTarefaORM>();

        services.AddEntityFrameworkConfig(configuration);

        return services;
    }

    private static void AddEntityFrameworkConfig(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        string? connectionString = configuration["SQL_CONNECTION_STRING"];

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new Exception("A variável SQL_CONNECTION_STRING não foi fornecida.");

        services.AddDbContext<IUnitOfWork, AppDbContext>(options =>
            options.UseNpgsql(connectionString, (opt) => opt.EnableRetryOnFailure(3)));
    }
}
