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
app.Run();
