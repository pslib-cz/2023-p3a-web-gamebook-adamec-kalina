﻿using System;
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
        private IGameLocationService _gameLocationServiceImplementation;

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

        public Dialog? GetDialog(Location location)
        {
            try
            {
                // Retrieve data from the session
                var serializedModel = _session.GetString($"{location}Dialog");
                if(serializedModel == null) return null;
                var dialogsList = JsonSerializer.Deserialize<List<Dialog>>(serializedModel);
                var playerFocus = GetPlayerFocus();
                var playerDealingType = GetPlayerDealingType();
                var gameProgress = GetGameProgress();
                
                    
                return dialogsList.FirstOrDefault(d => d.DialogOrder.Quest == gameProgress.Quest && d.DialogOrder.Step == gameProgress.Step && (d.DialogFocus == null || d.DialogFocus == playerFocus) && (d.SpecialType == null || d.SpecialType == playerDealingType));
            }
            catch (Exception e)
            {
                throw new Exception($"Dialog for [{location}] was not found -> {e.Message}");
            }
        }

        public Choice? GetChoice(Location location)
        {
            try
            {
                // Retrieve data from the session
                var serializedModel = _session.GetString($"{location}Choice");
                if(serializedModel == null) return null;
                var choice = JsonSerializer.Deserialize<Choice>(serializedModel);
                if (choice.Available) return choice;
                return null;
                // var gameProgress = GetGameProgress();
            }
            catch (Exception e)
            {
                throw new Exception($"Choice for [{location}] was not found -> {e.Message}");
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

        public PlayerDealingType? GetPlayerDealingType()
        {
            try
            {
                var playerDealingTypeString = _session.GetString("playerDealingType");
                if (!Enum.TryParse(playerDealingTypeString, true, out PlayerDealingType playerDealingType)) return null;
                return playerDealingType;
            }
            catch (Exception e)
            {
                throw new Exception($"Error while retrieving the playerDealingType from the session -> {e.Message}");
            }
            
        }

        public HitboxType? GetHitbox(Location location)
        {
            var gameLocation = GetLocation(location);
            var gameProgress = GetGameProgress();
            var playerDealingType = GetPlayerDealingType();
            if (gameLocation.Hitboxes == null) return null;
            var availableHitbox = gameLocation.Hitboxes.FirstOrDefault(h => (h.HitboxOrder == null || (h.HitboxOrder.Quest == gameProgress.Quest && h.HitboxOrder.Step == gameProgress.Step)) && (h.PlayerDealingType == playerDealingType || h.PlayerDealingType == null) && h.Available);
            return availableHitbox?.Type;
        }

        public List<Quest> GetQuests()
        {
            var serializedQuests = _session.GetString($"quests");
            return JsonSerializer.Deserialize<List<Quest>>(serializedQuests);
        }

        public List<TargetLocation> GetTargetLocationList(Location location)
        {
            return GetTargetLocations(location).Select(targetLocation => new TargetLocation()
            {
                Location = targetLocation,
                Locked = IsLocationLocked(targetLocation)
            }).ToList();
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

        public List<Item> GetInventoryItems()
        {
            try
            {
                // Retrieve data from the session
                var serializedModel = _session.GetString("inventory");
                return JsonSerializer.Deserialize<List<Item>>(serializedModel);
            }
            catch (Exception e)
            {
                throw new Exception($"Error while trying to get inventory -> {e.Message}");
            }
        }

        public GameProgress GetGameProgress()
        {
            try
            {
                // Retrieve data from the session
                var serializedModel = _session.GetString("gameProgress");
                return JsonSerializer.Deserialize<GameProgress>(serializedModel);
            }
            catch (Exception e)
            {
                throw new Exception($"Game progress -> {e.Message}");
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
            if (playerFocusString == null) return null; //Player has no focus yet
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

