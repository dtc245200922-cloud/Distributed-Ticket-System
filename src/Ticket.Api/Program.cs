using Microsoft.EntityFrameworkCore;
using Ticket.Api;
using Ticket.Shared;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TicketDb")
    ?? throw new InvalidOperationException("Missing ConnectionStrings:TicketDb");
builder.Services.AddDbContext<TicketDbContext>(o => o.UseNpgsql(connectionString));
var app = builder.Build();
await DatabaseSetup.InitializeAsync(app.Services, app.Logger);

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));
app.MapGet("/events", async (TicketDbContext db, string? search) =>
{
    var query = db.Events.AsNoTracking().AsQueryable();
    if (!string.IsNullOrWhiteSpace(search))
        query = query.Where(e => EF.Functions.ILike(e.Name, $"%{search}%"));
    return Results.Ok(await query.OrderBy(e => e.Id).ToListAsync());
});
app.MapGet("/events/{id:int}", async (int id, TicketDbContext db) =>
{
    var item = await db.Events.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    return item is null ? Results.NotFound(new { error = "Event not found" }) : Results.Ok(item);
});
app.Run();
