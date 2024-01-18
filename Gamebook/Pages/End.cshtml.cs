using System.Net.Mime;
using System.Runtime.InteropServices;
using Gamebook.Interfaces;
using Gamebook.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Global = Gamebook.GlobalModels.GlobalModels;

namespace Gamebook.Pages;

public class End : PageModel
{
    private readonly ISessionHelper _session;
    private readonly IGameLocationService _locationService;
    
    public EndPageResponse EndPageResponse { get; set; }

    public End(ISessionHelper session, IGameLocationService locationService)
    {
        _session = session;
        _locationService = locationService;
    }
    
    public void OnGet()
    {
        var moralScore = _locationService.GetPlayerMoralScore();

        // Create the response model
        EndPageResponse = new();
        //TODO adjust the values
        EndPageResponse.Ending = moralScore switch
        {
            (< 20) => Global.Endings.First(e => e.Key == 3).Value,
            (< 40) => Global.Endings.First(e => e.Key == 2).Value,
            _ => Global.Endings.First(e => e.Key == 1).Value
        };
    }
}