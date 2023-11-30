using System;
using Gamebook.Enums;
using Gamebook.Interfaces;
using Gamebook.Models;

namespace Gamebook.Services
{
    public class GameLocationService : IGameLocationService
    {
        private Dictionary<Location, GameLocationModel> gameLocationDataDict = new Dictionary<Location, GameLocationModel>
        {
            {Location.Cave, new GameLocationModel(){Title = "Cave", Description = "cave, btw ur moms a hoe", BackgroundImagePath = "caveImageRightHere"}},
            {Location.Forest, new GameLocationModel(){Title = "Forest", Description = "forest, btw ur moms a hoe", BackgroundImagePath = "forestImageRightHere"}},
            {Location.City, new GameLocationModel(){Title = "City", Description = "city, btw ur moms a hoe", BackgroundImagePath = "cityImageRightHere"}}
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

