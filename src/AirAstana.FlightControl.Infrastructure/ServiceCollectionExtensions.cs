using System;
using System.Reflection;
using System.Text;
using AirAstana.FlightControl.Application.Interfaces.Services;
using AirAstana.FlightControl.Domain.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AirAstana.FlightControl.Infrastructure.Persistence;
using AirAstana.FlightControl.Infrastructure.Services.Auth;
using AirAstana.FlightControl.Infrastructure.Services.Flight;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace AirAstana.FlightControl.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IWebHostEnvironment environment,
        IConfiguration configuration)
    {
        services
            .AddConfigurations(configuration)
            .JwtAuth(configuration)
            .AddTransientServices()
            .AddScopedServices(environment)
            .ConfigureInfrastructureRedisCache(configuration);

        return services;
    }

    #region private  ServiceCollectionExtensions

    /// <summary>
    /// DB connection string 
    /// </summary>
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContextImp>(options =>
            {
                options.UseNpgsql(connectionString, opt => { opt.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null); });
            });

        return services;
    }

    /// <summary>
    /// inject singleton configurations
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    private static IServiceCollection AddConfigurations(this IServiceCollection services,
        IConfiguration configuration)
    {
        //services.Configure<RedisKeysOptions>(configuration.GetSection(RedisKeysOptions.SectionName));
      
        return services;
    }

    /// <summary>
    /// inject transient lifecycle services
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    private static IServiceCollection AddTransientServices(this IServiceCollection services)
    {
        return services;
    }

    /// <summary>
    /// inject scope lifecycle services
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    private static IServiceCollection AddScopedServices(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddScoped<IAppDbContext, AppDbContextImp>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IFlightService, FlightService>();
        if (environment.IsDevelopment())
        {
            services.AddScoped<IAuthService, AuthServiceTest>();
            
        }
        else
        {
            services.AddScoped<IAuthService, AuthService>();
        }
        
        return services;
    }
    private static IServiceCollection ConfigureInfrastructureRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Redis");

        //services.AddStackExchangeRedisCache(options => { options.Configuration = connectionString; });

        return services;
    }

    private  static IServiceCollection JwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings["Key"]))
                };
            });
        services.AddAuthorization();
        return services;
    }

    #endregion
}