﻿using System;
using Gamebook.Enums;
using Gamebook.Models;

namespace Gamebook.Interfaces
{
    public interface IGameLocationService
    {
        GameLocation GetLocation(Location location);
        List<Location> GetTargetLocations(Location location);
        List<Dialog> GetDialog(Location location);
        PlayerStats GetPlayerStats();
        HitboxType? GetHitbox(Location location);
        List<Quest> GetQuests();
        List<TargetLocation> GetTargetLocationList(Location location);
        Weapon GetEquippedWeapon();
        GameProgress GetGameProgress();
        PlayerFocus? GetPlayerFocus();
        int GetPlayerMoralScore();
        bool IsValidConnection(Location locationFrom, Location locationTo);
        bool IsLocationLocked(Location location);
        Location GetCurrentLocation();
        void SetCurrentLocation(Location location);
        void ResetGame();
        bool IsGameInProgress();
        void SetGameInProgress();




    }
}

