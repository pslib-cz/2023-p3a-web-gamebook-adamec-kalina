using System;
using System.Text.Json;
using Gamebook.Enums;
using Gamebook.Interfaces;
using Gamebook.Models;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Gamebook.Services
{
    public class GameLocationService : IGameLocationService
    {
        private readonly ISessionHelper _session;
        
        public GameLocationService(ISessionHelper session)
        {
            _session = session;
        }

        public GameLocation GetLocation(Location location)
        {
            try
            {
                // Retrieve data from the session
                var serializedModel = _session.GetString(location.ToString());
                return JsonSerializer.Deserialize<GameLocation>(serializedModel);
            }
            catch(Exception e)
            {
                throw new Exception($"Location [{location}] was not found -> {e.Message}");
            }
        }

        public List<Dialog>? GetDialog(Location location)
        {
            try
            {
                // Retrieve data from the session
                var serializedModel = _session.GetString($"{location}Dialog");
                if(serializedModel == null) return null;
                var dialogsList = JsonSerializer.Deserialize<List<Dialog>>(serializedModel);
                var playerFocusString = _session.GetString("playerFocus");

                
                return !Enum.TryParse(playerFocusString, true, out PlayerFocus playerFocus) ? 
                    // No focus set yet
                    dialogsList.Where(d => d.Available).ToList() :
                    // Dialog for the specific focus
                    dialogsList.Where(d => d.Available && d.DialogFocus == playerFocus).ToList();
            }
            catch (Exception e)
            {
                throw new Exception($"Dialog for [{location}] was not found -> {e.Message}");
            }
        }

        public PlayerStats GetPlayerStats()
        {
            try
            {
                // Retrieve data from the session
                var serializedModel = _session.GetString("playerStats");
                return JsonSerializer.Deserialize<PlayerStats>(serializedModel);
            }
            catch (Exception e)
            {
                throw new Exception($"Player stats were not found -> {e.Message}");
            }
        }
        
        public Weapon GetEquippedWeapon()
        {
            try
            {
                // Retrieve data from the session
                var serializedModel = _session.GetString("equippedWeapon");
                return JsonSerializer.Deserialize<Weapon>(serializedModel);
            }
            catch (Exception e)
            {
                throw new Exception($"Equipped weapon was not found -> {e.Message}");
            }
        }

        public List<Location> GetTargetLocations(Location location)
        {
            try
            {
                var serializedModel = _session.GetString($"{location}TargetLocations");
                return JsonSerializer.Deserialize<List<Location>>(serializedModel);
            }
            catch (Exception e)
            {
                throw new Exception($"Target locations for [{location}] were not found -> {e.Message}");
            }
        }

        public Location GetCurrentLocation()
        {
            string currentLocation = string.Empty;
            try
            {
                currentLocation = _session.GetString("currentLocation");
                if (String.IsNullOrEmpty(currentLocation))
                {
                    throw new Exception("Error no current location was found");
                }

                return Enum.Parse<Location>(currentLocation);
            }
            catch (Exception e)
            {
                throw new Exception($"Error wrong currentLocation value -> {currentLocation}");
            }
        }

        public int GetPlayerMoralScore()
        {
            var serializedModel = _session.GetString("playerStats");
            return JsonSerializer.Deserialize<PlayerStats>(serializedModel).MoralScore;
        }

        public void SetCurrentLocation(Location location)
        {
            _session.SetString("currentLocation", location.ToString());
        }

        public PlayerFocus? GetPlayerFocus()
        {
            var playerFocusString = _session.GetString("playerFocus");
            if (!Enum.TryParse(playerFocusString, true, out PlayerFocus playerFocus))
            {
                throw new Exception("Invalid player focus");
            }

            return playerFocus;
        }

        public bool IsValidConnection(Location locationFrom, Location locationTo)
        {
            if (locationFrom == locationTo) return true;
            var targetLocations = GetTargetLocations(locationFrom);
            var connection = targetLocations.FirstOrDefault(t => t == locationTo);
            return connection != null && !IsLocationLocked(locationTo);
        }

        public bool IsLocationLocked(Location location)
        {
            try
            {
                var gameLocationString = _session.GetString(location.ToString());
                var gameLocation = JsonSerializer.Deserialize<GameLocation>(gameLocationString);
                return gameLocation.Locked;
            }
            catch (Exception e)
            {
                throw new Exception($"Error while retrieving a game location from session -> {e.Message}");
            }
        }

        public bool IsGameInProgress()
        {
            bool gameInProgress;
            try
            {
                var gameInProgressString = _session.GetString("gameInProgress");
                if (String.IsNullOrEmpty(gameInProgressString))
                {
                    throw new Exception("Error gameInProgress could not be retrieved from session");
                }
                return bool.Parse(gameInProgressString);
            }
            catch (Exception e)
            {
                throw new Exception("Error gameInProgress could not be retrieved from session");

            }
        }

        public void SetGameInProgress()
        {
            _session.SetString("gameInProgress", true.ToString());
        }

        public void ResetGame()
        {
            _session.SetSessionDefaultState();
        }


    }
}

