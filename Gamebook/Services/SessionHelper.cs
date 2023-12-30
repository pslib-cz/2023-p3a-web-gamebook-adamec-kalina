using System.Net.Mime;
using System.Reflection.Metadata;
using System.Text.Json;
using Gamebook.Enums;
using Gamebook.Interfaces;
using Gamebook.Models;

namespace Gamebook.Services;

public class SessionHelper : ISessionHelper
{
    private readonly IHttpContextAccessor _httpContext;
    
    //TODO Default game locations' states
    private Dictionary<Location, GameLocation> gameLocationDataDict = new()
    {
        {Location.SlumDistrict, new GameLocation(){Title = "Slum District", Description = "Slum District, btw ur moms a hoe", BackgroundImage = "slum-district"}},
        {Location.SlumQuarter, new GameLocation(){Title = "Slum Quarter", Description = "Slum Quarter, btw ur moms a hoe", BackgroundImage = "slum-quarter"}},
        {Location.ElectroShop, new GameLocation(){Title = "Electro Shop", Description = "Electro Shop, btw ur moms a hoe", BackgroundImage = "electro-shop"}},
        {Location.DarkAlley, new GameLocation(){Title = "Dark Alley", Description = "Dark Alley, btw ur moms a hoe", BackgroundImage = "dark-alley"}},
        {Location.ShadyBar, new GameLocation(){Title = "Shady Bar", Description = "Shady Bar, btw ur moms a hoe", BackgroundImage = "shady-bar"}},
        {Location.PartOfTheBar, new GameLocation(){Title = "Part of the Bar", Description = "Part of the Bar, btw ur moms a hoe", BackgroundImage = "part-of-the-bar"}},
        {Location.BackEntrance, new GameLocation(){Title = "Back Entrance", Description = "Back Entrance, btw ur moms a hoe", BackgroundImage = "back-entrance"}},
        {Location.SecretMeetingPlace, new GameLocation(){Title = "Secret Meeting Place", Description = "Secret Meeting Place, btw ur moms a hoe", BackgroundImage = "secret-meeting-place"}},
        {Location.Workshop, new GameLocation(){Title = "Workshop", Description = "Workshop, btw ur moms a hoe", BackgroundImage = "workshop"}},
        {Location.TacticalRoom, new GameLocation(){Title = "Tactical Room", Description = "Tactical Room, btw ur moms a hoe", BackgroundImage = "tactical-room"}},
        {Location.CyberLab, new GameLocation(){Title = "Cyber Lab", Description = "Cyber Lab, btw ur moms a hoe", BackgroundImage = "cyber-lab"}},
        {Location.QuantumTechnology, new GameLocation(){Title = "Quantum Technology", Description = "Quantum Technology, btw ur moms a hoe", BackgroundImage = "quantum-technology"}},
    };

    //TODO Default game locations dialog states
    private Dictionary<string, Dialog> gameLocationDialogDict = new()
    {
        {$"{Location.SlumDistrict}Dialog", new Dialog { Texts = new List<string> {"", "", ""}}},
        {$"{Location.SlumQuarter}Dialog", new Dialog {Texts = new List<string> {"", "", ""}}},
        {$"{Location.ElectroShop}Dialog", new Dialog {Texts = new List<string> {"", "", ""}}},
        {$"{Location.DarkAlley}Dialog", new Dialog {Texts = new List<string> {"", "", ""}}},
        {$"{Location.ShadyBar}Dialog", new Dialog {Texts = new List<string> {"", "", ""}}},
        {$"{Location.PartOfTheBar}Dialog", new Dialog {Texts = new List<string> {"", "", ""}}},
        {$"{Location.BackEntrance}Dialog", new Dialog {Texts = new List<string> {"", "", ""}}},
        {$"{Location.SecretMeetingPlace}Dialog", new Dialog {Texts = new List<string> {"", "", ""}}},
        {$"{Location.Workshop}Dialog", new Dialog {Texts = new List<string> {"", "", ""}}},
        {$"{Location.TacticalRoom}Dialog", new Dialog {Texts = new List<string> {"", "", ""}}},
        {$"{Location.CyberLab}Dialog", new Dialog {Texts = new List<string> {"", "", ""}}},
        {$"{Location.QuantumTechnology}Dialog", new Dialog {Texts = new List<string> {"", "", ""}}}
    };

    //TODO Default game locations' target locations states 
    private Dictionary<string, List<TargetLocation>> gameLocationTargetLocationDict = new()
    {
        {$"{Location.SlumDistrict}TargetLocations", new List<TargetLocation> {new() {Location = Location.SlumQuarter, Locked=false}, new() {Location = Location.DarkAlley}}},
        {$"{Location.SlumQuarter}TargetLocations", new List<TargetLocation> {new() {Location = Location.SlumDistrict, Locked = false}, new() {Location = Location.ElectroShop}}},
        {$"{Location.ElectroShop}TargetLocations", new List<TargetLocation> {new() {Location = Location.SlumQuarter}}},
        {$"{Location.DarkAlley}TargetLocations", new List<TargetLocation> {new() {Location = Location.SlumDistrict}}},
        {$"{Location.ShadyBar}TargetLocations", new List<TargetLocation> {new() {Location = Location.PartOfTheBar}, new(){Location = Location.BackEntrance}}},
        {$"{Location.PartOfTheBar}TargetLocations", new List<TargetLocation> {new() {Location = Location.ShadyBar}}},
        {$"{Location.BackEntrance}TargetLocations", new List<TargetLocation> {new() {Location = Location.ShadyBar}, new() {Location = Location.SecretMeetingPlace}}},
        {$"{Location.SecretMeetingPlace}TargetLocations", new List<TargetLocation> {new() {Location = Location.CyberLab}, new() { Location = Location.Workshop}, new() { Location = Location.TacticalRoom}}},
        {$"{Location.Workshop}TargetLocations", new List<TargetLocation> {new() {Location = Location.SecretMeetingPlace}, new() { Location = Location.QuantumTechnology}}},
        {$"{Location.TacticalRoom}TargetLocations", new List<TargetLocation> {new() {Location = Location.SecretMeetingPlace}}},
        {$"{Location.CyberLab}TargetLocations", new List<TargetLocation> {new() {Location = Location.SecretMeetingPlace}}},
        {$"{Location.QuantumTechnology}TargetLocations", new List<TargetLocation> {new() {Location = Location.Workshop}}}

    };
 
    //Default inventory state
    private List<Item> inventoryItemList = new()
    {
        //Empty inventory at the beginning 
    };

    //TODO Default quests state
    private List<Quest> questList = new()
    {
        //TODO quest 1
    };

    //TODO Default player state
    private PlayerStats playerStats = new()
    {
        Health = 50 , MaxHealth = 50, Energy = 50, MaxEnergy = 50, Money = 0
    };

    // Game not in progress by default
    private bool gameInProgress = false;

    // Set default currentLocation on game start
    private Location currentLocation = Location.SlumDistrict;

    //TODO default equipped weapon
    private Weapon equippedWeapon = new Weapon()
    {
        Type = WeaponType.Knife,
        Demage = 0,
        EnergyConsumption = 0
    };
    
    public SessionHelper(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
        SetSessionDefaultState();
    }

    /// <summary>
    /// Retrieves a value from the session
    /// </summary>
    /// <param name="key"></param>
    /// <returns> session value </returns>
    public string GetString(string key)
    {
        try
        {
            return _httpContext.HttpContext.Session.GetString(key);

        }
        catch (Exception e)
        {
            throw new Exception($"Error while trying to retrieve a value of [{key}] from the session -> {e.Message}");
        }
        
    }

    /// <summary>
    /// Stores a new key-value pair in the session
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetString(string key, string value)
    {
        try
        {
            _httpContext.HttpContext.Session.SetString(key, value);
        }
        catch (Exception e)
        {
            throw new Exception($"Error while trying to set a new pair [{key}, {value}] into the session -> {e.Message}");
        }
    }
    
    public void SetSessionDefaultState()
    {
        // Add game locations info
        foreach (var pair in gameLocationDataDict)
        {
            string serializedValue = JsonSerializer.Serialize(pair.Value);
            _httpContext.HttpContext.Session.SetString(pair.Key.ToString(), serializedValue);
        }

        // Add dialogs
        foreach (var pair in gameLocationDialogDict)
        {
            string serializedValue = JsonSerializer.Serialize(pair.Value);
            _httpContext.HttpContext.Session.SetString(pair.Key, serializedValue);
        }

        // Add target locations
        foreach (var pair in gameLocationTargetLocationDict)
        {
            string serializedValue = JsonSerializer.Serialize(pair.Value);
            _httpContext.HttpContext.Session.SetString(pair.Key, serializedValue);
        }

        // Add inventory items
        string serializedItemList = JsonSerializer.Serialize(inventoryItemList);
        _httpContext.HttpContext.Session.SetString("inventory", serializedItemList);

        // Add quests
        string serializedQuestList = JsonSerializer.Serialize(questList);
        _httpContext.HttpContext.Session.SetString("quests", serializedQuestList);

        // Add player stats
        string serializedPlayerStats = JsonSerializer.Serialize(playerStats);
        _httpContext.HttpContext.Session.SetString("playerStats", serializedPlayerStats);
        
        // Game not in progress
        _httpContext.HttpContext.Session.SetString("gameInProgress", gameInProgress.ToString());
        
        // Set default currentLocation
        _httpContext.HttpContext.Session.SetString("currentLocation", currentLocation.ToString());
        
        // Set default equipped weapon
        string serializedEquippedWeapon = JsonSerializer.Serialize(equippedWeapon);
        _httpContext.HttpContext.Session.SetString("equippedWeapon", serializedEquippedWeapon);
    }

}