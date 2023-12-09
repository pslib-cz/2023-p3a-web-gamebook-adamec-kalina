using System.Net.Mime;
using System.Text.Json;
using Gamebook.Enums;
using Gamebook.Interfaces;
using Gamebook.Models;

namespace Gamebook.Services;

public class SessionHelper : ISessionHelper
{
    private readonly IHttpContextAccessor _httpContext;
    
    // Default game locations' states
    private Dictionary<Location, GameLocationModel> gameLocationDataDict = new()
    {
        {Location.SlumDistrict, new GameLocationModel(){Title = "Slum District", Description = "Slum District, btw ur moms a hoe", BackgroundImage = "slum-district"}},
        {Location.SlumQuarter, new GameLocationModel(){Title = "Slum Quarter", Description = "Slum Quarter, btw ur moms a hoe", BackgroundImage = "slum-quarter"}},
        {Location.ElectroShop, new GameLocationModel(){Title = "Electro Shop", Description = "Electro Shop, btw ur moms a hoe", BackgroundImage = "electro-shop"}},
        {Location.DarkAlley, new GameLocationModel(){Title = "Dark Alley", Description = "Dark Alley, btw ur moms a hoe", BackgroundImage = "dark-alley"}},
        {Location.ShadyBar, new GameLocationModel(){Title = "Shady Bar", Description = "Shady Bar, btw ur moms a hoe", BackgroundImage = "shady-bar"}},
        {Location.PartOfTheBar, new GameLocationModel(){Title = "Part of the Bar", Description = "Part of the Bar, btw ur moms a hoe", BackgroundImage = "part-of-the-bar"}},
        {Location.BackEntrance, new GameLocationModel(){Title = "Back Entrance", Description = "Back Entrance, btw ur moms a hoe", BackgroundImage = "back-entrance"}},
        {Location.SecretMeetingPlace, new GameLocationModel(){Title = "Secret Meeting Place", Description = "Secret Meeting Place, btw ur moms a hoe", BackgroundImage = "secret-meeting-place"}},
        {Location.Workshop, new GameLocationModel(){Title = "Workshop", Description = "Workshop, btw ur moms a hoe", BackgroundImage = "workshop"}},
        {Location.TacticalRoom, new GameLocationModel(){Title = "Tactical Room", Description = "Tactical Room, btw ur moms a hoe", BackgroundImage = "tactical-room"}},
        {Location.CyberLab, new GameLocationModel(){Title = "Cyber Lab", Description = "Cyber Lab, btw ur moms a hoe", BackgroundImage = "cyber-lab"}},
        {Location.QuantumTechnology, new GameLocationModel(){Title = "Quantum Technology", Description = "Quantum Technology, btw ur moms a hoe", BackgroundImage = "quantum-technology"}},
    };

    // Default game locations dialog states
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

    private Dictionary<string, List<TargetLocation>> gameLocationTargetLocationDict = new()
    {
        {$"{Location.SlumDistrict}TargetLocations", new List<TargetLocation> {}},
        {$"{Location.SlumQuarter}TargetLocations", new List<TargetLocation> {}},
        {$"{Location.ElectroShop}TargetLocations", new List<TargetLocation> {}},
        {$"{Location.DarkAlley}TargetLocations", new List<TargetLocation> {}},
        {$"{Location.ShadyBar}TargetLocations", new List<TargetLocation> {}},
        {$"{Location.PartOfTheBar}TargetLocations", new List<TargetLocation> {}},
        {$"{Location.BackEntrance}TargetLocations", new List<TargetLocation> {}},
        {$"{Location.SecretMeetingPlace}TargetLocations", new List<TargetLocation> {}},
        {$"{Location.Workshop}TargetLocations", new List<TargetLocation> {}},
        {$"{Location.TacticalRoom}TargetLocations", new List<TargetLocation> {}},
        {$"{Location.CyberLab}TargetLocations", new List<TargetLocation> {}},
        {$"{Location.QuantumTechnology}TargetLocations", new List<TargetLocation> {}}

    };
 
    // Default inventory state
    private List<Item> inventoryItemList = new()
    {
        Item.Item1, Item.Item2
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
    /// <returns></returns>
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