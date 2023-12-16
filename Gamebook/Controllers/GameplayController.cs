using Gamebook.Interfaces;
using Gamebook.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gamebook.Controllers;

[ApiController]
[Route("[controller]")]
public class GameplayController : ControllerBase
{
    private readonly IGameplayService _gameplayService;
    
    public GameplayController(IGameplayService gameplayService)
    {
        _gameplayService = gameplayService;
    }

    [HttpPost]
    public ActionResult HealthChange([FromBody]int ammount)
    {
        _gameplayService.HealthChange(ammount);
        return Ok();
    }
}