using System;
using Gamebook.Enums;
using Gamebook.Models;

namespace Gamebook.Interfaces
{
    public interface IGameLocationService
    {
        public GameLocation GetLocation(Location location);
        public List<TargetLocation> GetTargetLocations(Location location);
        public Dialog GetDialog(Location location);
        public bool IsValidConnection(Location locationFrom, Location locationTo);

    }
}

