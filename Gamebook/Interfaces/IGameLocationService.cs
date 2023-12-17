using System;
using Gamebook.Enums;
using Gamebook.Models;

namespace Gamebook.Interfaces
{
    public interface IGameLocationService
    {
        GameLocation GetLocation(Location location);
        List<TargetLocation> GetTargetLocations(Location location);
        Dialog GetDialog(Location location);
        PlayerStats GetPlayerStats();
        Weapon GetEquippedWeapon();
        bool IsValidConnection(Location locationFrom, Location locationTo);
        Location GetCurrentLocation();
        void SetCurrentLocation(Location location);
        void ResetGame();
        bool IsGameInProgress();
        void SetGameInProgress();




    }
}

