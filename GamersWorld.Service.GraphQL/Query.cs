using GamersWorld.Data;
using GamersWorld.DataContext;

namespace GamersWorld.Service.GraphQL;

/*
    GraphQL için örnek sorgular

    query getGames{
        games{
            title,
            point,
            releaseYear,
            onSale
        }
    }

    query find {
        game(gameId: 7) {
            title
            onSale
            releaseYear
            point
        }
    }

*/
public class Query
{
    public IQueryable<Game> GetGames(GamersWorldDbContext context) => context.Games;

    public Game? GetGame(GamersWorldDbContext context, int gameId)
    {
        var game = context.Games.Find(gameId);
        if (game != null)
        {
            return game;
        }
        return null;
    }
}