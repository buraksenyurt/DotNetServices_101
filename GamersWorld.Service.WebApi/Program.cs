using GamersWorld.Data;
using GamersWorld.DataContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGamersWorld();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapPost("/games", async (Game game, GamersWorldDbContext context) =>
{
    context.Games.Add(game);
    await context.SaveChangesAsync();

    return Results.Created($"/games/{game.GameId}", game);
});

app.MapGet("/games", async (GamersWorldDbContext context) => await context.Games.ToListAsync());

app.MapGet("/games/{id}"
    , async (int id, GamersWorldDbContext context) =>
        await context.Games.FindAsync(id) is Game game ? Results.Ok(game) : Results.NotFound());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
