using System.Reflection;
using AirAstana.FlightControl.Application.Features.Behaviors;
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
        .AddTransientServices()
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
    
    /// <summary>
    /// AutoMapper
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    private static IServiceCollection AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        return services;
    }
    
    /// <summary>
    /// inject transient lifecycle services
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    private static IServiceCollection AddTransientServices(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        return services;
    }
}