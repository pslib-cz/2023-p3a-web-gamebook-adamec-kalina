using Gamebook.Interfaces;
using Gamebook.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Gamebook.Controllers;

[ApiController]
[Route("[controller]")]
public class GameplayController : ControllerBase
{
    // private readonly IGameplayService _gameplayService;
    //
    // public GameplayController(IGameplayService gameplayService)
    // {
    //     _gameplayService = gameplayService;
    // }

    [HttpPost("HealthChange")]
    public ActionResult HealthChange([FromBody]int amount)
    {
        if (amount <= 0)
        {
            return Ok(new {redirectToDeath = "/Death"});
        }

        Console.WriteLine("success");
        return Ok();
    }    
}