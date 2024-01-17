using Gamebook.Enums;
using Gamebook.Interfaces;
using Gamebook.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gamebook.Pages;

public class Map : PageModel
{
    private readonly IGameLocationService _locationService;
    public MapPageResponse MapPageResponse { get; set; }

    public Map(IGameLocationService locationService)
    {
        _locationService = locationService;
    }
    
    public void OnGet()
    {
        MapPageResponse = new()
        {
            CurrentLocation = _locationService.GetCurrentLocation(),
            QuestList = _locationService.GetQuests(),
            FastTravelLocations = new FastTravelLocationsModel()
            {
                SlumDistrict = !_locationService.IsLocationLocked(Location.SlumDistrict),
                ShadyBar = !_locationService.IsLocationLocked(Location.ShadyBar),
                SecretMeetingPlace = !_locationService.IsLocationLocked(Location.SecretMeetingPlace),
            }
        };
        
    }
}