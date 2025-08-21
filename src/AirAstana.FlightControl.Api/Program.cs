using AirAstana.FlightControl.Application;
using AirAstana.FlightControl.Infrastructure;
using AirAstana.FlightControl.Infrastructure.Logging;
using AirAstana.FlightControl.Infrastructure.Persistence;
using AirAstana.FlightControl.Infrastructure.Swagger;
using Serilog;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

try
{
    builder.Host.UseSerilogLogging(builder.Configuration);
    builder.Services.AddApplicationOptions(builder.Configuration);
    builder.Services.AddInfrastructure(builder.Environment, builder.Configuration)
        .AddPersistence(builder.Configuration)
        .AddSwaggerWithAuth().
        AddSwaggerExamplesFromAssemblyOf<Program>();
    
    
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContextImp>();
            await DbInitializer.SeedAsync(db);
        }
        
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "AirAstana Flight Control v1");
        });
    }
    
    app.UseRouting();  
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

