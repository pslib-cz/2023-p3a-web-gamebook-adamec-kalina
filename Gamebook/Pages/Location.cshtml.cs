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
        private readonly IGameplayService _gameplayService;

        public LocationPageResponse LocationPageResponse { get; set; }
        
        
        public LocationModel(IGameLocationService locationService, IGameplayService gameplayService)
        {
            _locationService = locationService;
            _gameplayService = gameplayService;
        }


        public IActionResult OnGet(string location)
        {
            var previousLocation = _locationService.GetCurrentLocation();
            bool valid;

            if (Enum.TryParse<Location>(location, out var currentLocation) && Enum.IsDefined(typeof(Location), currentLocation))
            {
                // Checking the location connection validity
                valid = _locationService.IsValidConnection(previousLocation, currentLocation);
            }
            else
            {
                valid = false;
            }

            if (!valid)
            {
                // Redirect back if the location connection not allowed
                return RedirectToPage("/Location", new { location = previousLocation.ToString() });
            }
            
            // Set the new location as current
            if (previousLocation != currentLocation)
            {
                _locationService.SetCurrentLocation(currentLocation); 
            }

            #region exceptions location locking

            switch (currentLocation)
            {
                case Location.ExtractionVan:
                    _gameplayService.LockLocation(Location.CorporalBuilding);
                    break;
                case Location.EscapeCar:
                    _gameplayService.LockLocation(Location.Facility);
                    break;
                case Location.Helipad:
                    _gameplayService.LockLocation(Location.DataDepot);
                    break;
            }

            #endregion
            

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
            var gameLocation = _locationService.GetLocation(currentLocation);
            ViewData["LocationTitle"] = gameLocation.Title;
            ViewData["BackgroundImage"] = gameLocation.BackgroundImage;

            var playerStats = _locationService.GetPlayerStats();
            ViewData["PlayerHealth"] = playerStats.Health;
            ViewData["PlayerMaxHealth"] = playerStats.MaxHealth;
            ViewData["PlayerEnergy"] = playerStats.Energy;
            ViewData["PlayerMaxEnergy"] = playerStats.MaxEnergy;

            ViewData["PlayerHealthPercentage"] = (playerStats.Health > 0) ? (playerStats.Health * 100) / playerStats.MaxHealth : 0;
            ViewData["PlayerEnergyPercentage"] = (playerStats.Energy > 0) ? (playerStats.Energy * 100) / playerStats.MaxEnergy : 0;
            

            LocationPageResponse = new()
            {
                TargetLocations = _locationService.GetTargetLocationList(currentLocation),
                Dialog = _locationService.GetDialog(currentLocation),
                EquipedWeapon = _locationService.GetEquippedWeapon(),
                PlayerStats = _locationService.GetPlayerStats(),
                Hitbox = _locationService.GetHitbox(currentLocation).ToString(),
                Choices = _locationService.GetChoice(currentLocation)
            };
        }
    }
}
