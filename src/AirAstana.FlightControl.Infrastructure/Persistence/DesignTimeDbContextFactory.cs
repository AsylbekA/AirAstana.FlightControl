using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AirAstana.FlightControl.Infrastructure.Persistence;


public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContextImp>
{
    public AppDbContextImp CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContextImp>();

        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;");

        return new AppDbContextImp(optionsBuilder.Options);
    }
}