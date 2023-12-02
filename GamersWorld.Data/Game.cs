namespace GamersWorld.Data;

public class Game
{
    public int GameId { get; set; }
    public string Title { get; set; } = "Anoynmous";
    public double Point { get; set; }
    public short ReleaseYear { get; set; }
    public bool OnSale { get; set; }
}
