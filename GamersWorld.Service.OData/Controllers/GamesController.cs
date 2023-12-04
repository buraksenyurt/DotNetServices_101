using GamersWorld.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace GamersWorld.Service.OData.Contollers;

public class GamesController(GamersWorldDbContext context)
    : ODataController
{
    private readonly GamersWorldDbContext _context = context;

    [EnableQuery]
    public IActionResult Get()
    {
        return Ok(_context.Games.OrderBy(g => g.Title));
    }

    [EnableQuery]
    public IActionResult Get(int key)
    {
        return Ok(_context.Games.Where(g => g.GameId == key));
    }
}