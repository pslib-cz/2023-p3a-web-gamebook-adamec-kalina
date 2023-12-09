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
        {$"{Location.SlumDistrict}Dialog", new Dialog {Text = ""}},
        {$"{Location.SlumQuarter}Dialog", new Dialog {Text = ""}},
        {$"{Location.ElectroShop}Dialog", new Dialog {Text = ""}},
        {$"{Location.DarkAlley}Dialog", new Dialog {Text = ""}},
        {$"{Location.ShadyBar}Dialog", new Dialog {Text = ""}},
        {$"{Location.PartOfTheBar}Dialog", new Dialog {Text = ""}},
        {$"{Location.BackEntrance}Dialog", new Dialog {Text = ""}},
        {$"{Location.SecretMeetingPlace}Dialog", new Dialog {Text = ""}},
        {$"{Location.Workshop}Dialog", new Dialog {Text = ""}},
        {$"{Location.TacticalRoom}Dialog", new Dialog {Text = ""}},
        {$"{Location.CyberLab}Dialog", new Dialog {Text = ""}},
        {$"{Location.QuantumTechnology}Dialog", new Dialog {Text = ""}}
    };

    //TODO Default game locations' target locations states 
    private Dictionary<string, List<TargetLocation>> gameLocationTargetLocationDict = new()
    {
        {$"{Location.SlumDistrict}TargetLocations", new List<TargetLocation> {new() {Location = Location.SlumQuarter}, new() {Location = Location.ElectroShop}, new() {Location = Location.DarkAlley}}},
        {$"{Location.SlumQuarter}TargetLocations", new List<TargetLocation> {new() {Location = Location.SlumDistrict}}},
        {$"{Location.ElectroShop}TargetLocations", new List<TargetLocation> {new() {Location = Location.SlumDistrict}}},
        {$"{Location.DarkAlley}TargetLocations", new List<TargetLocation> {new() {Location = Location.SlumDistrict}}},
        {$"{Location.ShadyBar}TargetLocations", new List<TargetLocation> {new() {Location = Location.PartOfTheBar}, new(){Location = Location.BackEntrance}}},
        {$"{Location.PartOfTheBar}TargetLocations", new List<TargetLocation> {new() {Location = Location.ShadyBar}}},
        {$"{Location.BackEntrance}TargetLocations", new List<TargetLocation> {new() {Location = Location.ShadyBar}, new() {Location = Location.SecretMeetingPlace}}},
        {$"{Location.SecretMeetingPlace}TargetLocations", new List<TargetLocation> {new() {Location = Location.BackEntrance}}},
        {$"{Location.Workshop}TargetLocations", new List<TargetLocation> {}},
        {$"{Location.TacticalRoom}TargetLocations", new List<TargetLocation> {}},
        {$"{Location.CyberLab}TargetLocations", new List<TargetLocation> {}},
        {$"{Location.QuantumTechnology}TargetLocations", new List<TargetLocation> {}}

    };
 
    //TODO Default inventory state
    private List<Item> inventoryItemList = new()
    {
        Item.Battery, Item.Item2
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
    
    private void SetSessionDefaultState()
    {
        foreach (var pair in gameLocationDataDict)
        {
            string serializedValue = JsonSerializer.Serialize(pair.Value);
            _httpContext.HttpContext.Session.SetString(pair.Key.ToString(), serializedValue);
        }

        foreach (var pair in gameLocationDialogDict)
        {
            string serializedValue = JsonSerializer.Serialize(pair.Value);
            _httpContext.HttpContext.Session.SetString(pair.Key, serializedValue);
        }

        foreach (var pair in gameLocationTargetLocationDict)
        {
            string serializedValue = JsonSerializer.Serialize(pair.Value);
            _httpContext.HttpContext.Session.SetString(pair.Key, serializedValue);
        }

        string serializedList = JsonSerializer.Serialize(inventoryItemList);
        _httpContext.HttpContext.Session.SetString("inventory", serializedList);
    }

}