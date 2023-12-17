using Gamebook.Enums;
using Gamebook.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gamebook.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IGameLocationService _locationService;
    
    public bool GameInProgress { get; set; }

    public IndexModel(IGameLocationService locationService, ILogger<IndexModel> logger)
    {
        _logger = logger;
        _locationService = locationService;
    }

    public void OnGet()
    {
        
        var currentLocation = _locationService.GetCurrentLocation();
        ViewData["lastLocation"] = currentLocation;

        GameInProgress = _locationService.IsGameInProgress();
    }
}
