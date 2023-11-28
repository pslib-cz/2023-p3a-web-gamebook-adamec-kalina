using System;
using Gamebook.Enums;
using Gamebook.Models;

namespace Gamebook.Interfaces
{
    public interface IGameLocationService
    {
        public GameLocationModel GetLocation(Location location);
    }
}

