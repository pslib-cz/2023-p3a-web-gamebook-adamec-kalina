using Gamebook.Enums;
using Gamebook.Interfaces;
using Gamebook.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gamebook.Pages;

public class LoadOut : PageModel
{
    private readonly IGameLocationService _locationService;
    public LoadOutPageResponse LoadOutPageResponse { get; set; }

    public LoadOut(IGameLocationService locationService)
    {
        _locationService = locationService;
    }
    
    public void OnGet()
    {
        LoadOutPageResponse = new()
        {
            CurrentLocation = _locationService.GetCurrentLocation(),
            PlayerFocus = _locationService.GetPlayerFocus()
        };
        
    }
}