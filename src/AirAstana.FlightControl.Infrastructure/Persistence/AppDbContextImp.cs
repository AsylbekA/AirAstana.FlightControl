using System.Threading.Tasks;
using AirAstana.FlightControl.Domain.Entities;
using AirAstana.FlightControl.Domain.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AirAstana.FlightControl.Infrastructure.Persistence;

public sealed class AppDbContextImp: DbContext, IAppDbContext
{
    #region Constructor
   public AppDbContextImp(DbContextOptions<AppDbContextImp> options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }

    #endregion

    #region Methods

    public async Task<int> SaveChangesAsync() => await base.SaveChangesAsync();

    #endregion

    #region DbSet

    public DbSet<Flight> Flights{ get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles{ get; set; } = null!;
    
    #endregion
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Flight
        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(f => f.Id);
            entity.Property(f => f.Origin).HasMaxLength(256).IsRequired();
            entity.Property(f => f.Destination).HasMaxLength(256).IsRequired();
            entity.Property(f => f.Status).HasConversion<string>().IsRequired();
        });

        // Role
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.HasIndex(r => r.Code).IsUnique();
            entity.Property(r => r.Code).HasMaxLength(256).IsRequired();
        });

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Username).IsUnique();
            entity.Property(u => u.Username).HasMaxLength(256).IsRequired();
            entity.Property(u => u.Password).HasMaxLength(256).IsRequired();

            entity.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}