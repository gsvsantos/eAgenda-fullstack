using eAgenda.Core.Aplicacao;
using eAgenda.Infraestrutura.ORM;
using eAgenda.WebAPI.AutoMapper;
using eAgenda.WebAPI.Identity;
using eAgenda.WebAPI.ORM;
using eAgenda.WebAPI.Swagger;

namespace eAgenda.WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddCamadaAplicacao(builder.Logging, builder.Configuration)
            .AddCamadaInfraestruturaOrm(builder.Configuration);

        builder.Services.AddAutoMapperProfiles(builder.Configuration);

        builder.Services.AddIdentityProviders();
        builder.Services.AddJwtAuthentication(builder.Configuration);

        builder.Services.AddControllers();

        builder.Services.AddSwaggerConfig();

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.ApplyMigrations();

            app.UseSwagger();
            app.UseSwaggerUI(options =>  // Define a UI do Swagger como a rota padrão da aplicação.
            {                                      // Tip pega em > https://aka.ms/aspnetcore/swashbuckle
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
