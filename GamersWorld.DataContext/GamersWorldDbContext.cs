using GamersWorld.Data;
using Microsoft.EntityFrameworkCore;

namespace GamersWorld.DataContext;

public partial class GamersWorldDbContext : DbContext
{
    public GamersWorldDbContext()
    {
    }

    public GamersWorldDbContext(DbContextOptions<GamersWorldDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=gamersworld;Username=johndoe;Password=somew0rds");
        }
    }

    public virtual DbSet<Game> Games { get; set; }
}
