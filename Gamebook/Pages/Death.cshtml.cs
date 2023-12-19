using Gamebook.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gamebook.Pages;

public class Death : PageModel
{
    private readonly IGameLocationService _locationService;

    public Death(IGameLocationService locationService)
    {
        _locationService = locationService;
    }
    
    public void OnGet()
    {
        _locationService.ResetGame();
    }
}