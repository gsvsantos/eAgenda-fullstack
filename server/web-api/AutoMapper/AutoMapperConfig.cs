using System.Reflection;

namespace eAgenda.WebAPI.AutoMapper;

public static class AutoMapperConfig
{
    public static IServiceCollection AddAutoMapperProfiles(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        Assembly assembly = typeof(AutoMapperConfig).Assembly;

        string? luckyPennySoftwareLicenseKey = configuration["LUCKYPENNYSOFTWARE_LICENSE_KEY"];

        services.AddAutoMapper(config => config.LicenseKey = luckyPennySoftwareLicenseKey, assembly);

        return services;
    }
}
