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
    /// Sets player's dealing type 
    /// </summary>
    /// <param name="playerDealingType"></param>
    public void SetPlayerDealingType(PlayerDealingType playerDealingType)
    {
        _session.SetString("playerDealingType", playerDealingType.ToString());
    }

    /// <summary>
    /// Increases the gameProgress (dialog is over => progress in the game)
    /// Unlocks the next quest in case the dialog was the last one in its quest
    /// </summary>
    /// <exception cref="Exception"></exception>
    public void DialogOver()
    {
        var currentLocation = _locationService.GetCurrentLocation();
        var playerFocus = _locationService.GetPlayerFocus();
        var playerDealingType = _locationService.GetPlayerDealingType();
        try
        {
            var gameProgress = _locationService.GetGameProgress();
            var locationDialogsString = _session.GetString($"{currentLocation}Dialog");
            var locationDialogs = JsonSerializer.Deserialize<List<Dialog>>(locationDialogsString);

            if (locationDialogs == null) return;
            var finishedDialog = locationDialogs.First(d => d.DialogOrder.Quest == gameProgress.Quest &&
                                                            d.DialogOrder.Step == gameProgress.Step &&
                                                            (d.DialogFocus == playerFocus || d.DialogFocus == null) && // playerFocus check
                                                            (d.SpecialType == null || d.SpecialType == playerDealingType)); // playerDealingType check
            if (finishedDialog.DialogOrder.LastStep)
            {
                // Unlock the next quest
                UnlockNextQuest(gameProgress.Quest);
                gameProgress.Quest++;
                gameProgress.Step = 1;
            }
            else
            {
                if (finishedDialog.SpecialType == PlayerDealingType.Peaceful) {gameProgress.Step += 2;} // Just a stupid exception for the one peaceful choice in quest 1 :)
                else{gameProgress.Step++;}
            }

            finishedDialog.Unlock?.ForEach(UnlockLocation);
            finishedDialog.ItemsAdd?.ForEach(AddItem);
            finishedDialog.ItemsRemove?.ForEach(RemoveItem);

            // Save the progress back into the session
            string serializedGameProgress= JsonSerializer.Serialize(gameProgress);
            _session.SetString("gameProgress", serializedGameProgress);
            
            
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
            var playerDealingType = _locationService.GetPlayerDealingType();
            
            
            if (location.Hitboxes.Count == 0 || location.Hitboxes.FirstOrDefault(h => h.Available && (h.PlayerDealingType == null || h.PlayerDealingType == playerDealingType)) == null) return;//No hitboxes at all or just no available ones on the current page
    
            var hitbox = location.Hitboxes.First(h => h.Available && (h.PlayerDealingType == null || h.PlayerDealingType == playerDealingType));
            if(hitbox.Type == HitboxType.Pin)UnlockLocation(Location.SecretMeetingPlace); //an exception for the pin hitbox -> the only occurrence where a hitbox unlocks a location :)
            // Change the hitbox available state
            location.Hitboxes.First(h => h.Available && (h.PlayerDealingType == null || h.PlayerDealingType == playerDealingType)).Available = false;
                
            // Save the changed location info back into the session
            string serializedLocation= JsonSerializer.Serialize(location);
            _session.SetString(currentLocation.ToString(), serializedLocation);
        }
        catch (Exception e)
        {
            throw new Exception($"Error while trying to set a hitbox in [{currentLocation}] unavailable -> {e.Message}");
        }    
    }

    public void SetChoiceNotAvailable()
    {
        var currentLocation = _locationService.GetCurrentLocation();
        try
        {
            var choice = _locationService.GetChoice(currentLocation);
            if (choice.Available) choice.Available = false;
            else{return;}
                
            // Save the changed location info back into the session
            string serializedChoice= JsonSerializer.Serialize(choice);
            _session.SetString($"{currentLocation}Choice", serializedChoice);
        }
        catch (Exception e)
        {
            throw new Exception($"Error while trying to set a choice in [{currentLocation}] unavailable -> {e.Message}");
        }    
    }

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
    /// Sets a specific location as Locked = true
    /// </summary>
    /// <param name="location"></param>
    /// <exception cref="Exception"></exception>
    public void LockLocation(Location location)
    {
        try
        {
            var gameLocationString = _session.GetString(location.ToString());
            var gameLocation = JsonSerializer.Deserialize<GameLocation>(gameLocationString);
            
            if (gameLocation.Locked) return;
            gameLocation.Locked = true;
            string serializedGameLocation = JsonSerializer.Serialize(gameLocation);
            _session.SetString(location.ToString(), serializedGameLocation);
        }
        catch (Exception e)
        {
            throw new Exception($"Error while retrieving a game location from session -> {e.Message}");
        }
    }

    private void UnlockNextQuest(int questCompletedNum)
    {
        var currentQuests = _locationService.GetQuests();
        currentQuests.First(q => q.Number == questCompletedNum).Completed = true;
        var nextQuest = Global.Quests.FirstOrDefault(q => q.Number == questCompletedNum + 1); 
        if(nextQuest != null){currentQuests.Add(nextQuest);}
        
        // Set the modified quest list back into the session
        string serializedQuestList = JsonSerializer.Serialize(currentQuests);
        _session.SetString("quests", serializedQuestList);
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

    public void AddItem(Item item)
    {
        var inventoryString = _session.GetString("inventory");

        try
        {
            var inventory = JsonSerializer.Deserialize<List<Item>>(inventoryString);
            // Add the new item
            inventory.Add(item);
            string serializedInventory = JsonSerializer.Serialize(inventory);
            // Save the modified inventory
            _session.SetString("inventory", serializedInventory);
        }
        catch (Exception e)
        {
            throw new Exception($"Error while adding an item -> {e.Message}");
        }
    }
    public void RemoveItem(Item item)
    {
        var inventoryString = _session.GetString("inventory");
        try
        {
            var inventory = JsonSerializer.Deserialize<List<Item>>(inventoryString);
            if (!inventory.Contains(item))
            {
                throw new Exception($"item [{item}] cannot be removed since it does not appear to be in the inventory");
            }
            // Remove the item
            inventory.Remove(item);
            string serializedInventory = JsonSerializer.Serialize(inventory);
            // Save the modified inventory
            _session.SetString("inventory", serializedInventory);
        }
        catch (Exception e)
        {
            throw new Exception($"Error while removing an item -> {e.Message}");
        }
    }

}