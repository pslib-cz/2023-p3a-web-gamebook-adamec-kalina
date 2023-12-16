using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gamebook.Enums;
using Gamebook.Interfaces;
using Gamebook.Models;
using Gamebook.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gamebook.Pages
{
    public class LocationModel : PageModel
    {
        private readonly IGameLocationService _locationService;

        // public GameLocationModel Location { get; private set; }
        public LocationPageResponse LocationPageResponse { get; set; }
        
        
        public LocationModel(IGameLocationService locationService)
        {
            _locationService = locationService;
        }


        public void OnGet(string location)
        {
            var previousLocationTempData = TempData["currentLocation"];
            if(string.IsNullOrEmpty(previousLocationTempData.ToString())){previousLocationTempData = location;}
            var previousLocation = Enum.Parse<Location>(previousLocationTempData.ToString());
            var currentlocation = Enum.Parse<Location>(location);
            // Checking the location connection validity 
            bool valid = _locationService.IsValidConnection(previousLocation, currentlocation);
            TempData["currentLocation"] = $"{location}"; // Setting the new location as the current
            
            var gameLocation = _locationService.GetLocation(currentlocation);//parse the string into the Enum
            ViewData["LocationTitle"] = gameLocation.Title;
            ViewData["LocationDescription"] = gameLocation.Description;
            ViewData["BackgroundImage"] = gameLocation.BackgroundImage;

            LocationPageResponse = new()
            {
                TargetLocations = _locationService.GetTargetLocations(currentlocation),
                Dialog = _locationService.GetDialog(currentlocation)
            };

            // return null;
        }

        private IActionResult Redirect(Location location)
        {
            return RedirectToPage("Location", new { location = location.ToString() });
        }
    }
}
