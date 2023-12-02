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
            {Location.ChudinskeCtvrti, new GameLocationModel(){Title = "Chudinské Čtvrti", Description = "Chudinské Čtvrti, btw ur moms a hoe", BackgroundImageClass = "caveImageRightHere"}},
            {Location.ChudinskaCtvrt, new GameLocationModel(){Title = "Chudinská Čtvrť", Description = "Chudinská Čtvrť, btw ur moms a hoe", BackgroundImageClass = "caveImageRightHere"}},
            {Location.KramekSElektronikou, new GameLocationModel(){Title = "Krámek s Elektronikou", Description = "Krámek s Elektronikou, btw ur moms a hoe", BackgroundImageClass = "caveImageRightHere"}},
            {Location.TemnaUlicka, new GameLocationModel(){Title = "Temná Ulička", Description = "Temná Ulička, btw ur moms a hoe", BackgroundImageClass = "caveImageRightHere"}},
            {Location.ZapadlyBar, new GameLocationModel(){Title = "Zapadlý Bar", Description = "Zapadlý Bar, btw ur moms a hoe", BackgroundImageClass = "caveImageRightHere"}},
            {Location.CastBaru, new GameLocationModel(){Title = "Část Baru", Description = "Část Baru, btw ur moms a hoe", BackgroundImageClass = "caveImageRightHere"}},
            {Location.ZadniVchod, new GameLocationModel(){Title = "Zadní Vchod", Description = "Zadní Vchod, btw ur moms a hoe", BackgroundImageClass = "caveImageRightHere"}},
            {Location.TajneSchuzoveMisto, new GameLocationModel(){Title = "Tajné Schůzové Místo", Description = "Tajné Schůzové Místo, btw ur moms a hoe", BackgroundImageClass = "caveImageRightHere"}},
            {Location.Dilna, new GameLocationModel(){Title = "Dílna", Description = "Dílna, btw ur moms a hoe", BackgroundImageClass = "caveImageRightHere"}},
            {Location.TaktickaMistnost, new GameLocationModel(){Title = "Taktická Místnost", Description = "Taktická Místnost, btw ur moms a hoe", BackgroundImageClass = "caveImageRightHere"}},
            {Location.Laborator, new GameLocationModel(){Title = "Laboratoř", Description = "Laboratoř, btw ur moms a hoe", BackgroundImageClass = "caveImageRightHere"}},
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

