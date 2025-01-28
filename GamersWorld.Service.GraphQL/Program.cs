using GamersWorld.DataContext;
using GamersWorld.Service.GraphQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGamersWorld();

builder.Services
    .AddGraphQLServer()
    .RegisterDbContextFactory<GamersWorldDbContext>()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();

var app = builder.Build();

app.MapGet("/", () => "GraphQl için /graphql diyin");

app.MapGraphQL();

app.Run();
