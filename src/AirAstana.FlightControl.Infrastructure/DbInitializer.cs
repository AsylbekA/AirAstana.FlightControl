using System.Linq;
using System.Threading.Tasks;
using AirAstana.FlightControl.Domain.Entities;
using AirAstana.FlightControl.Domain.Persistence;
using AirAstana.FlightControl.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AirAstana.FlightControl.Infrastructure;

public static class DbInitializer
{
    public static async Task SeedAsync(AppDbContextImp context)
    {
        // убедимся, что база создана
        await context.Database.MigrateAsync();

        // роли
        if (!await context.Roles.AnyAsync())
        {
            var clientRole = new Role { Code = "Client" };
            var moderatorRole = new Role { Code = "Moderator" };
            await context.Roles.AddRangeAsync(clientRole, moderatorRole);
            await context.SaveChangesAsync();

            // пользователи
            var moderator = new User
            {
                Username = "Moderator",
                Password = BCrypt.Net.BCrypt.HashPassword("Moderator123!"),
                RoleId = moderatorRole.Id
            };
            var client = new User
            {
                Username = "Client",
                Password = BCrypt.Net.BCrypt.HashPassword("Client123!"),
                RoleId = clientRole.Id
            };

            await context.Users.AddRangeAsync(moderator, client);
            await context.SaveChangesAsync();
        }
    }
}