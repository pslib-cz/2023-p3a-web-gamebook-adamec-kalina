using System.Text.Json;
using Gamebook.Enums;
using Gamebook.Interfaces;
using Gamebook.Models;
using Gamebook.Pages;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Global = Gamebook.GlobalModels.GlobalModels;

namespace Gamebook.Services;

public class GameplayService : IGameplayService
{
    private readonly ISessionHelper _session;
    private readonly IGameLocationService _locationService;

    public GameplayService(ISessionHelper session, IGameLocationService locationService)
    {
        _session = session;
        _locationService = locationService;
    }
    
    /// <summary>
    /// Changes player's health
    /// </summary>
    /// <param name="amountLeft"> the amount of health left </param>
    public void HealthChange(int amountLeft)
    {
        var serializedModel = _session.GetString("playerStats");
        PlayerStats playerStats = JsonSerializer.Deserialize<PlayerStats>(serializedModel);
        playerStats.Health = amountLeft;
        string serializedPlayerStats = JsonSerializer.Serialize(playerStats);
        _session.SetString("playerStats", serializedPlayerStats);
    }

    /// <summary>
    /// Changes player's energy
    /// </summary>
    /// <param name="amountLeft"> the amount of energy left </param>
    public void EnergyChange(int amountLeft)
    {
        var serializedModel = _session.GetString("playerStats");
        PlayerStats playerStats = JsonSerializer.Deserialize<PlayerStats>(serializedModel);
        playerStats.Energy = amountLeft;
        string serializedPlayerStats = JsonSerializer.Serialize(playerStats);
        _session.SetString("playerStats", serializedPlayerStats);
    }

    /// <summary>
    /// Changes player's money amount
    /// </summary>
    /// <param name="amountLeft"> the amount of money left </param>
    public void MoneyChange(int amountLeft)
    {
        var serializedModel = _session.GetString("playerStats");
        PlayerStats playerStats = JsonSerializer.Deserialize<PlayerStats>(serializedModel);
        playerStats.Money = amountLeft;
        string serializedPlayerStats = JsonSerializer.Serialize(playerStats);
        _session.SetString("playerStats", serializedPlayerStats);
    }

    /// <summary>
    /// Changes player's moral score
    /// </summary>
    /// <param name="changeAmount"> the amount to be added </param>
    public void MoralScoreChange(int changeAmount)
    {
        var serializedModel = _session.GetString("playerStats");
        PlayerStats playerStats = JsonSerializer.Deserialize<PlayerStats>(serializedModel);
        playerStats.MoralScore += changeAmount;
        string serializedPlayerStats = JsonSerializer.Serialize(playerStats);
        _session.SetString("playerStats", serializedPlayerStats);
    }

    /// <summary>
    /// Sets player's story focus
    /// </summary>
    /// <param name="playerFocus"></param>
    public void SetPlayerFocusChoice(PlayerFocus playerFocus)
    {
        _session.SetString("playerFocus", playerFocus.ToString());
    }

    /// <summary>
    /// Sets a specific dialog as not available
    /// </summary>
    /// <exception cref="Exception"></exception>
    public void SetDialogNotAvailable()
    {
        var currentLocation = _locationService.GetCurrentLocation();
        var playerFocus = _locationService.GetPlayerFocus();
        try
        {
            var locationDialogsString = _session.GetString($"{currentLocation}Dialog");
            var locationDialogs = JsonSerializer.Deserialize<List<Dialog>>(locationDialogsString);
            locationDialogs.First(d => d.Available && (d.DialogFocus == null || d.DialogFocus == playerFocus)).Available = false;
            
            string serializedLocationDialogs= JsonSerializer.Serialize(locationDialogs);
            _session.SetString($"{currentLocation}Dialog", serializedLocationDialogs);
        }
        catch (Exception e)
        {
            throw new Exception($"Error while trying to set a dialog in [{currentLocation}] unavailable -> {e.Message}");
        }
    }

    public void SetHitboxNotAvailable()
    {
        var currentLocation = _locationService.GetCurrentLocation();
        try
        {
            var location = _locationService.GetLocation(currentLocation);
            if (location.Hitboxes.Count == 0 || location.Hitboxes.FirstOrDefault(h => h.Available) == null) return;//No hitboxes at all or just no available ones on the current page

            // Change the hitbox available state
            location.Hitboxes.First(h => h.Available).Available = false;
                
            // Save the changed location info back into the session
            string serializedLocation= JsonSerializer.Serialize(location);
            _session.SetString(currentLocation.ToString(), serializedLocation);
        }
        catch (Exception e)
        {
            throw new Exception($"Error while trying to set a hitbox in [{currentLocation}] unavailable -> {e.Message}");
        }    }

    /// <summary>
    /// Sets a specific location as Locked = false
    /// </summary>
    /// <param name="location"></param>
    /// <exception cref="Exception"></exception>
    public void UnlockLocation(Location location)
    {
        try
        {
            var gameLocationString = _session.GetString(location.ToString());
            var gameLocation = JsonSerializer.Deserialize<GameLocation>(gameLocationString);
            
            if (!gameLocation.Locked) return;
            gameLocation.Locked = false;
            string serializedGameLocation = JsonSerializer.Serialize(gameLocation);
            _session.SetString(location.ToString(), serializedGameLocation);
        }
        catch (Exception e)
        {
            throw new Exception($"Error while retrieving a game location from session -> {e.Message}");
        }
    }

    /// <summary>
    /// Changes the player's equipped weapon + de-equips the current one
    /// </summary>
    /// <param name="type"></param>
    public void EquipWeapon(WeaponType type)
    {
        // Retrieve the new weapon from global models
        var weapon = Global.Weapons.First(w => w.Type == type);
        
        string serializedEquippedWeapon = JsonSerializer.Serialize(weapon);
        _session.SetString("equippedWeapon", serializedEquippedWeapon);
    }

    /// <summary>
    /// Sets a quest as completed
    /// </summary>
    /// <param name="number"> number of the quest </param>
    public void SetQuestCompleted(int number)
    {
        // Retrieve current quest list from the session
        var questsString = _session.GetString("quests");
        var quests = JsonSerializer.Deserialize<List<Quest>>(questsString);

        // Set the quest as completed
        try
        {
            quests.First(q => q.Number == number).Completed = true;
        }
        catch (Exception e)
        {
            throw new Exception($"Error - Quest number [{number}] was not found");
        }
        // Save back into the session
        string serializedQuests = JsonSerializer.Serialize(quests);
        _session.SetString("quests", serializedQuests);
    }

    /// <summary>
    /// Adds a new quest into the current quests list
    /// </summary>
    /// <param name="number"> number of the quest to be added </param>
    public void AddNewQuest(int number)
    {
        // Retrieve players focus from the session
        var playerFocus = _locationService.GetPlayerFocus();
        
        // Retrieve current quest list from the session
        var questsString = _session.GetString("quests");
        var quests = JsonSerializer.Deserialize<List<Quest>>(questsString);
        
        // Add the new quest from global models
        try
        {
            quests.Add(Global.Quests.First(q => q.Number == number && (q.Focus == null || q.Focus == playerFocus)));
        }
        catch (Exception e)
        {
            throw new Exception($"Error - Quest number [{number}] was not found");
        }
        // Save back into the session
        string serializedQuests = JsonSerializer.Serialize(quests);
        _session.SetString("quests", serializedQuests);
        
    }

    public void GetItem(Item item)
    {
        try
        {
            // TODO
        }
        catch (Exception e)
        {
            throw new Exception($"Error while getting item -> {e.Message}");
        }
    }
    public void TakeItem(Item item)
    {
        try
        {
            // TODO
        }
        catch (Exception e)
        {
            throw new Exception($"Error while taking item -> {e.Message}");
        }
    }

}