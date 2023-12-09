using System.Text.Json;
using Gamebook.Enums;
using Gamebook.Interfaces;
using Gamebook.Models;

namespace Gamebook.Services;

public class SessionHelper : ISessionHelper
{
    private readonly IHttpContextAccessor _httpContext;
    
    // Default game locations' states
    private Dictionary<Location, GameLocationModel> gameLocationDataDict = new Dictionary<Location, GameLocationModel>
    {
        {Location.ChudinskeCtvrti, new GameLocationModel(){Title = "Chudinské Čtvrti", Description = "Chudinské Čtvrti, btw ur moms a hoe", BackgroundImage = "chudinske-ctvrti"}},
        {Location.ChudinskaCtvrt, new GameLocationModel(){Title = "Chudinská Čtvrť", Description = "Chudinská Čtvrť, btw ur moms a hoe", BackgroundImage = "chudinska-ctvrt"}},
        {Location.KramekSElektronikou, new GameLocationModel(){Title = "Krámek s Elektro", Description = "Krámek s Elektronikou, btw ur moms a hoe", BackgroundImage = "kramek-s-elektronikou"}},
        {Location.TemnaUlicka, new GameLocationModel(){Title = "Temná Ulička", Description = "Temná Ulička, btw ur moms a hoe", BackgroundImage = "temna-ulicka"}},
        {Location.ZapadlyBar, new GameLocationModel(){Title = "Zapadlý Bar", Description = "Zapadlý Bar, btw ur moms a hoe", BackgroundImage = "zapadly-bar"}},
        {Location.CastBaru, new GameLocationModel(){Title = "Část Baru", Description = "Část Baru, btw ur moms a hoe", BackgroundImage = "cast-baru"}},
        {Location.ZadniVchod, new GameLocationModel(){Title = "Zadní Vchod", Description = "Zadní Vchod, btw ur moms a hoe", BackgroundImage = "zadni-vchod"}},
        {Location.TajneSchuzoveMisto, new GameLocationModel(){Title = "Tajné Schůzové Místo", Description = "Tajné Schůzové Místo, btw ur moms a hoe", BackgroundImage = "tajne-schuzkove-misto"}},
        {Location.Dilna, new GameLocationModel(){Title = "Dílna", Description = "Dílna, btw ur moms a hoe", BackgroundImage = "dilna"}},
        {Location.TaktickaMistnost, new GameLocationModel(){Title = "Taktická Místnost", Description = "Taktická Místnost, btw ur moms a hoe", BackgroundImage = "takticka-mistnost"}},
        {Location.Laborator, new GameLocationModel(){Title = "Laboratoř", Description = "Laboratoř, btw ur moms a hoe", BackgroundImage = "laborator"}},
    };

    // Default inventory state
    private List<Item> gameItemsList = new List<Item>
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
    }

}