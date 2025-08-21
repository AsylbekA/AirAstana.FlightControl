using AirAstana.FlightControl.Application;
using AirAstana.FlightControl.Infrastructure;
using AirAstana.FlightControl.Infrastructure.Logging;
using AirAstana.FlightControl.Infrastructure.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

try
{
    builder.Host.UseSerilogLogging(builder.Configuration);
    builder.Services.AddApplicationOptions(builder.Configuration);
    builder.Services.AddInfrastructure(builder.Environment, builder.Configuration)
        .AddPersistence(builder.Configuration);
    
    
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContextImp>();
            await DbInitializer.SeedAsync(db);
        }
        
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseSerilogRequestLogging();
    app.UseMiddleware<ExceptionMiddleware>();
    app.MapControllers();
    app.Run();
    
}catch(Exception ex)
{
    Console.WriteLine(ex.Message);
    //Log.Logger.Error(ex, "Error when starting the project ({Application})...", builder.Environment.ApplicationName);
}
finally
{
   // await Log.CloseAndFlushAsync();
}

