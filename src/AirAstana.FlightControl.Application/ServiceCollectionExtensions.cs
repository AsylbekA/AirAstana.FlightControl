using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AirAstana.FlightControl.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationOptions(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddConfigurations(configuration)
        .AddMemoryCache()
        .AddMapper()
        .AddMediatR(Assembly.GetExecutingAssembly())
        .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
    
    private static IServiceCollection AddConfigurations(this IServiceCollection services,
        IConfiguration configuration)
    {
        //services.Configure<CoreRedisKeysOptions>(configuration.GetSection(CoreRedisKeysOptions.SectionName));
        return services;
    }
    private static IServiceCollection AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        return services;
    }

}