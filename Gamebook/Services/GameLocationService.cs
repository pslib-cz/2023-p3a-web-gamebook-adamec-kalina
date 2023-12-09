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
            {Location.SlumDistrict, new GameLocationModel(){Title = "Slum District", Description = "Slum District, btw ur moms a hoe", BackgroundImageClass = "slum-district"}},
            {Location.SlumQuarter, new GameLocationModel(){Title = "Slum Quarter", Description = "Slum Quarter, btw ur moms a hoe", BackgroundImageClass = "slum-quarter"}},
            {Location.ElectroShop, new GameLocationModel(){Title = "Electro Shop", Description = "Electro Shop, btw ur moms a hoe", BackgroundImageClass = "electro-shop"}},
            {Location.DarkAlley, new GameLocationModel(){Title = "Dark Alley", Description = "Dark Alley, btw ur moms a hoe", BackgroundImageClass = "dark-alley"}},
            {Location.ShadyBar, new GameLocationModel(){Title = "Shady Bar", Description = "Shady Bar, btw ur moms a hoe", BackgroundImageClass = "shady-bar"}},
            {Location.PartOfTheBar, new GameLocationModel(){Title = "Part of the Bar", Description = "Part of the Bar, btw ur moms a hoe", BackgroundImageClass = "part-of-the-bar"}},
            {Location.BackEntrance, new GameLocationModel(){Title = "Back Entrance", Description = "Back Entrance, btw ur moms a hoe", BackgroundImageClass = "back-entrance"}},
            {Location.SecretMeetingPlace, new GameLocationModel(){Title = "Secret Meeting Place", Description = "Secret Meeting Place, btw ur moms a hoe", BackgroundImageClass = "secret-meeting-place"}},
            {Location.Workshop, new GameLocationModel(){Title = "Workshop", Description = "Workshop, btw ur moms a hoe", BackgroundImageClass = "workshop"}},
            {Location.TacticalRoom, new GameLocationModel(){Title = "Tactical Room", Description = "Tactical Room, btw ur moms a hoe", BackgroundImageClass = "tactical-room"}},
            {Location.CyberLab, new GameLocationModel(){Title = "Cyber Lab", Description = "Cyber Lab, btw ur moms a hoe", BackgroundImageClass = "cyber-lab"}},
            {Location.QuantumTechnology, new GameLocationModel(){Title = "Quantum Technology", Description = "Quantum Technology, btw ur moms a hoe", BackgroundImageClass = "quantum-technology"}},
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

