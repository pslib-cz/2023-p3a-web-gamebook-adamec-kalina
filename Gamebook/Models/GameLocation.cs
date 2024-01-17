using System;
using System.Security.Cryptography.X509Certificates;
using Gamebook.Enums;

namespace Gamebook.Models
{
    public class GameLocation
    {
        public string Title { get; set; }
        public string BackgroundImage { get; set; }
        public bool Locked { get; set; } = true; // locked by default
        public List<Hitbox> Hitboxes { get; set; }
    }
}

