using GamersWorld.Data;
using GamersWorld.DataContext;
using GamersWorld.Service.WebApi.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging();
builder.Services.AddSingleton<IKeycloakTokenService, KeycloakTokenService>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://localhost:8380/realms/GamersWorld";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogError("JWT Authentication Failed: {}", context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                var tokenService = context.HttpContext.RequestServices.GetRequiredService<IKeycloakTokenService>();
                tokenService.ProcessValidatedToken(context.Principal);
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("SuperUsersOnly", policy => policy.RequireRole("SuperRole"))
    .AddPolicy("CustomersOnly", policy => policy.RequireRole("CustomerRole"));

builder.Services.AddGamersWorld();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/games", async (Game game, GamersWorldDbContext context) =>
{
    context.Games.Add(game);
    await context.SaveChangesAsync();
    return Results.Created($"/games/{game.GameId}", game);
})
.RequireAuthorization("SuperUsersOnly");

app.MapGet("/games", async (GamersWorldDbContext context) =>
{
    return await context.Games.ToListAsync();
})
.RequireAuthorization();

app.MapGet("/games/{id}", async (int id, GamersWorldDbContext context) =>
{
    return await context.Games.FindAsync(id) is Game game ? Results.Ok(game) : Results.NotFound();
})
.RequireAuthorization("CustomersOnly");

app.Run();
