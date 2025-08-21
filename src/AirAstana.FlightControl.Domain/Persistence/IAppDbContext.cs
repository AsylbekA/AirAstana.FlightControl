using System.Threading;
using System.Threading.Tasks;
using AirAstana.FlightControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirAstana.FlightControl.Domain.Persistence;

public interface IAppDbContext
{
    Task<int> SaveChangesAsync(CancellationToken ct);

    #region DbSet
    
    public DbSet<Flight> Flights{ get; set; }
    public DbSet<User> Users{ get; set; }
    public DbSet<Role> Roles { get; set; }
    
    #endregion
}