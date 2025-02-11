using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GamersWorld.DataContext;

public static class GamersWorldDbContextExtensions
{
    public static IServiceCollection AddGamersWorld(this IServiceCollection services
    , string connStr = "Host=localhost;Database=gamersworld;Username=johndoe;Password=somew0rds")
    {
        services.AddDbContext<GamersWorldDbContext>(options =>
            {
                options.UseNpgsql(connStr);
                options.LogTo(Console.WriteLine, [
                    Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting
                    ]
                );
            });

        return services;
    }
}