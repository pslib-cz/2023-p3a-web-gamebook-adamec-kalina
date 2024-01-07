using System.Text.Json;
using Gamebook.Enums;
using Gamebook.Interfaces;
using Gamebook.Models;
using Gamebook.Pages;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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
    
    public void HealthChange(int change)
    {
        var serializedModel = _session.GetString("playerStats");
        PlayerStats playerStats = JsonSerializer.Deserialize<PlayerStats>(serializedModel);
        playerStats.Health = change;
        string serializedPlayerStats = JsonSerializer.Serialize(playerStats);
        _session.SetString("playerStats", serializedPlayerStats);
    }

    public void EnergyChange(int change)
    {
        var serializedModel = _session.GetString("playerStats");
        PlayerStats playerStats = JsonSerializer.Deserialize<PlayerStats>(serializedModel);
        playerStats.Energy = change;
        string serializedPlayerStats = JsonSerializer.Serialize(playerStats);
        _session.SetString("playerStats", serializedPlayerStats);
    }

    public void MoneyChange(int change)
    {
        var serializedModel = _session.GetString("playerStats");
        PlayerStats playerStats = JsonSerializer.Deserialize<PlayerStats>(serializedModel);
        playerStats.Money = change;
        string serializedPlayerStats = JsonSerializer.Serialize(playerStats);
        _session.SetString("playerStats", serializedPlayerStats);
    }

    public void SetPlayerFocusChoice(PlayerFocus playerFocus)
    {
        _session.SetString("playerFocus", playerFocus.ToString());
    }

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
            throw new Exception($"Error while trying to set a dialog unavailable -> {e.Message}");
        }
    }

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
}