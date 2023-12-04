using GamersWorld.Client.Grpc;
using Grpc.Net.Client;

using var channel = GrpcChannel.ForAddress("http://localhost:5034");
var service = new GameBox.GameBoxClient(channel);

var addedGame = service.AddGame(new AddGameRequest
{
    Title = "1942",
    Point = 8.5,
    OnSale = true,
    ReleaseDate = 1984
});

Console.WriteLine($"{addedGame.GameId} - {addedGame.Title}");

Console.WriteLine("Tüm oyunlar");
var games = service.GetGames(new GetGamesRequest()).Data;

foreach (var game in games)
{
    Console.WriteLine($"{game.GameId} - {game.Title}");
}
