using Gamebook.Enums;
using Gamebook.Interfaces;
using Gamebook.Services;
using Microsoft.AspNetCore.Http.HttpResults;
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

    [HttpPost("HealthChange")]
    public ActionResult HealthChange([FromBody]int amount)
    {
        if (amount <= 0)
        {
            return Ok(new {redirectToDeath = "/Death"});
        }

        // Change the value in the session
        _gameplayService.HealthChange(amount);

        return Ok();
    }

    [HttpPost("EnergyChange")]
    public ActionResult EnergyChange([FromBody] int amount)
    {
        _gameplayService.EnergyChange(amount);
        return Ok();
    }
    
    [HttpPost("MoneyChange")]
    public ActionResult MoneyChange([FromBody] int amount)
    {
        _gameplayService.MoneyChange(amount);
        return Ok();
    }
    
    [HttpPost("MoralScoreChange")]
    public ActionResult MoralScoreChange([FromBody] int changeAmount)
    {
        _gameplayService.MoralScoreChange(changeAmount);
        return Ok();
    }

    [HttpPost("PlayerFocusChoice")]
    public ActionResult PlayerFocusChoice([FromBody] string playerFocusString)
    {
        if (!Enum.TryParse(playerFocusString, true, out PlayerFocus playerFocus))
            throw new Exception($"invalid player focus -> {playerFocusString}");

        _gameplayService.SetPlayerFocusChoice(playerFocus);
        
        return Ok();
    }

    [HttpPost("SetDialogNotAvailable")]
    public ActionResult SetDialogNotAvailable()
    {
        _gameplayService.SetDialogNotAvailable();
        
        return Ok();
    }

    [HttpPost("UnlockLocation")]
    public ActionResult UnlockLocation([FromBody] string locationString)
    {
        if (!Enum.TryParse(locationString, true, out Location location))
            throw new Exception($"invalid location -> {locationString}");

        _gameplayService.UnlockLocation(location);
        
        return Ok();
    }
    
    [HttpPost("EquipWeapon")]
    public ActionResult EquipWeapon([FromBody] string weaponTypeString)
    {
        if (!Enum.TryParse(weaponTypeString, true, out WeaponType weaponType))
            throw new Exception($"invalid weaponType -> {weaponTypeString}");

        _gameplayService.EquipWeapon(weaponType);
        
        return Ok();
    }
    
    [HttpPost("SetQuestCompleted")]
    public ActionResult SetQuestCompleted([FromBody] int questNumber)
    {
        _gameplayService.SetQuestCompleted(questNumber);
        return Ok();
    }
    
    [HttpPost("AddNewQuest")]
    public ActionResult AddNewQuest([FromBody] int questNumber)
    {
        _gameplayService.AddNewQuest(questNumber);
        return Ok();
    }
    
    
    
}