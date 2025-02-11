using GamersWorld.Data;
using GamersWorld.DataContext;

namespace GamersWorld.Service.GraphQL;

/*
    Mutation için örnek graphql sorgusu

    mutation {
        addGame(
            payload: {
            title: "Tower Defense"
            point: 5.4
            releaseYear: 2005
            onSale: true
            }
        ) {
            game {
            gameId
            title
            onSale
            releaseYear
            point
            }
        }
    }

*/

public record AddGameRequest(
    string Title,
    double Point,
    short ReleaseYear,
    bool OnSale
);

public class AddGameResponse(Game game)
{
    public Game Game { get; } = game;
}
public class Mutation
{
    public async Task<AddGameResponse> AddGameAsync(AddGameRequest payload, GamersWorldDbContext context)
    {
        Game game = new()
        {
            Title = payload.Title,
            Point = payload.Point,
            ReleaseYear = payload.ReleaseYear,
            OnSale = payload.OnSale
        };
        context.Games.Add(game);
        await context.SaveChangesAsync();
        return new AddGameResponse(game);
    }
}