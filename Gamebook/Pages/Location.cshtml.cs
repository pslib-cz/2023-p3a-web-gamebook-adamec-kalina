using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gamebook.Enums;
using Gamebook.Interfaces;
using Gamebook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gamebook.Pages
{
    public class LocationModel : PageModel
    {
        private readonly IGameLocationService _locationService;

        // public GameLocationModel Location { get; private set; }
        public LocationPageRequest LocationPageRequest { get; set; }
        
        
        public LocationModel(IGameLocationService locationService)
        {
            _locationService = locationService;
        }


        public void OnGet(string location)
        {
            var locationEnum = Enum.Parse<Location>(location);
            var gameLocation = _locationService.GetLocation(locationEnum);//parse the string into the Enum
            ViewData["LocationTitle"] = gameLocation.Title;
            ViewData["LocationDescription"] = gameLocation.Description;
            ViewData["BackgroundImage"] = gameLocation.BackgroundImage;

            LocationPageRequest = new()
            {
                TargetLocations = _locationService.GetTargetLocations(locationEnum),
                Dialog = _locationService.GetDialog(locationEnum)
            };
        }
    }
}
