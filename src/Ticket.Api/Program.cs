var app = WebApplication.CreateBuilder(args).Build();
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));
app.Run();
