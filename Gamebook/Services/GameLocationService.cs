using System;
using Gamebook.Enums;
using Gamebook.Interfaces;
using Gamebook.Models;

namespace Gamebook.Services
{
    public class GameLocationService : IGameLocationService
    {
        public Dictionary<Location, GameLocationModel> gameLocationDataDict = new Dictionary<Location, GameLocationModel>
        {
            //TODO list of ALL game locations
        };

        public GameLocationModel GetLocation(Location location)
        {
            GameLocationModel desiredLocation;
            try
            {
                desiredLocation = gameLocationDataDict[location];
            }
            catch(Exception e)
            {
                throw new Exception($"desired location was not found -> {e.Message}");
            }
            return desiredLocation;
        }
    }
}

