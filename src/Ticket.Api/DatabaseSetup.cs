using Microsoft.EntityFrameworkCore;
using Ticket.Shared;

namespace Ticket.Api;

public static class DatabaseSetup
{
    public static async Task InitializeAsync(IServiceProvider services, ILogger logger)
    {
        for (var attempt = 1; attempt <= 30; attempt++)
        {
            try
            {
                using var scope = services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<TicketDbContext>();
                await db.Database.EnsureCreatedAsync();
                if (!await db.Events.AnyAsync())
                {
                    db.Events.AddRange(
                        new Event { Name = "Ngay hoi cong nghe", Venue = "Thai Nguyen", StartsAtUtc = new DateTime(2027, 4, 15, 9, 0, 0, DateTimeKind.Utc), TotalTickets = 50, AvailableTickets = 50 },
                        new Event { Name = "Dem nhac sinh vien", Venue = "Ha Noi", StartsAtUtc = new DateTime(2027, 5, 12, 12, 0, 0, DateTimeKind.Utc), TotalTickets = 3, AvailableTickets = 3 },
                        new Event { Name = "Hoi thao Lap trinh C#", Venue = "Online", StartsAtUtc = new DateTime(2027, 6, 1, 2, 0, 0, DateTimeKind.Utc), TotalTickets = 1, AvailableTickets = 1 });
                    db.Users.AddRange(
                        new Customer { FullName = "Nguyen Van A", Email = "user1@example.test" },
                        new Customer { FullName = "Tran Thi B", Email = "user2@example.test" },
                        new Customer { FullName = "Le Van C", Email = "user3@example.test" });
                    await db.SaveChangesAsync();
                }
                logger.LogInformation("Database ready with sample data");
                return;
            }
            catch (Exception ex) when (attempt < 30)
            {
                logger.LogWarning(ex, "Database not ready: attempt {Attempt}/30", attempt);
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }
        throw new InvalidOperationException("Database initialization failed");
    }
}
