using Grpc.Core;
using GamersWorld.DataContext;
using GamersWorld.Data;

namespace GamersWorld.Service.Grpc.Services;

public class GameBoxService(ILogger<GameBoxService> logger, GamersWorldDbContext gamersWorldDbContext)
        : GameBox.GameBoxBase
{
    private readonly ILogger<GameBoxService> _logger = logger;
    private readonly GamersWorldDbContext _gamersWorldDbContext = gamersWorldDbContext;

    public override Task<GameMessage> GetGame(GetGameRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Get game request");
        GameMessage gameMessage = new();

        var game = _gamersWorldDbContext.Games.Where(g => g.GameId == request.Id).FirstOrDefault();
        if (game != null)
        {
            gameMessage.GameId = game.GameId;
            gameMessage.Title = game.Title;
            gameMessage.ReleaseDate = game.ReleaseYear;
            gameMessage.Point = game.Point;
            gameMessage.OnSale = game.OnSale;
        }

        return Task.FromResult(gameMessage);
    }

    public override Task<GetGamesReply> GetGames(GetGamesRequest request, ServerCallContext context)
    {
        GetGamesReply reply = new();

        var games = _gamersWorldDbContext.Games.OrderBy(g => g.Title).Select(g => new GameMessage
        {
            GameId = g.GameId,
            Title = g.Title,
            Point = g.Point,
            ReleaseDate = g.ReleaseYear,
            OnSale = g.OnSale
        });

        reply.Data.AddRange(games);
        return Task.FromResult(reply);
    }

    public override Task<GameMessage> AddGame(AddGameRequest addGameRequest, ServerCallContext context)
    {
        var game = new Game
        {
            Title = addGameRequest.Title,
            ReleaseYear = Convert.ToInt16(addGameRequest.ReleaseDate),
            Point = addGameRequest.Point,
            OnSale = addGameRequest.OnSale
        };
        _gamersWorldDbContext.Games.Add(game);
        _gamersWorldDbContext.SaveChanges();
        var response = new GameMessage
        {
            GameId = game.GameId,
            Title = game.Title,
            ReleaseDate = game.ReleaseYear,
            Point = game.Point,
            OnSale = game.OnSale
        };
        return Task.FromResult(response);
    }
}
