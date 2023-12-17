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

        public Dialog GetDialog(Location location)
        {
            try
            {
                // Retrieve data from the session
                var serializedModel = _session.GetString($"{location}Dialog");
                return JsonSerializer.Deserialize<Dialog>(serializedModel);
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

        public List<TargetLocation> GetTargetLocations(Location location)
        {
            try
            {
                var serializedModel = _session.GetString($"{location}TargetLocations");
                return JsonSerializer.Deserialize<List<TargetLocation>>(serializedModel);
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

        public void SetCurrentLocation(Location location)
        {
            _session.SetString("currentLocation", location.ToString());
        }

        public bool IsValidConnection(Location locationFrom, Location locationTo)
        {
            if (locationFrom == locationTo) return true;
            // if (locationTo is Location.SlumDistrict or Location.ShadyBar or Location.SecretMeetingPlace) return true;
            var targetLocations = GetTargetLocations(locationFrom);
            var connection = targetLocations.FirstOrDefault(t => t.Location == locationTo && !t.Locked);
            return connection != null;
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

