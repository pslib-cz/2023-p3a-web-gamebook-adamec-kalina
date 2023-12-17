﻿using System;
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


        public IActionResult OnGet(string location)
        {
            var previousLocation = _locationService.GetCurrentLocation();
            var currentLocation = Enum.Parse<Location>(location);
            
            // Checking the location connection validity 
            bool valid = _locationService.IsValidConnection(previousLocation, currentLocation);
            
            if (!valid)
            {
                return RedirectToPage("/Location", new { location = previousLocation.ToString() });
            }
            
            // Set the new location as current
            if (previousLocation != currentLocation)
            {
                _locationService.SetCurrentLocation(currentLocation); 
            }
            
            PassLocationData(currentLocation);

            return Page();
        }

        public void OnGetStart(string location)
        {
            var currentLocation = Enum.Parse<Location>(location);
            _locationService.ResetGame();
            _locationService.SetGameInProgress();
            
            // Set the new location as current
            _locationService.SetCurrentLocation(currentLocation); 
            
            PassLocationData(currentLocation);
        }

        private void PassLocationData(Location currentLocation)
        {
            var gameLocation = _locationService.GetLocation(currentLocation);//parse the string into the Enum
            ViewData["LocationTitle"] = gameLocation.Title;
            ViewData["LocationDescription"] = gameLocation.Description;
            ViewData["BackgroundImage"] = gameLocation.BackgroundImage;

            LocationPageResponse = new()
            {
                TargetLocations = _locationService.GetTargetLocations(currentLocation),
                Dialog = _locationService.GetDialog(currentLocation),
                EquipedWeapon = _locationService.GetEquippedWeapon(),
                PlayerStats = _locationService.GetPlayerStats()
            };
        }
    }
}
