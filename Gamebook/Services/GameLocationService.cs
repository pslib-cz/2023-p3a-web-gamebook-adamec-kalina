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

        public GameLocationModel GetLocation(Location location)
        {
            try
            {
                // Retrieve data from the session
                var serializedModel = _session.GetString(location.ToString());
                return JsonSerializer.Deserialize<GameLocationModel>(serializedModel);
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

        
    }
}

