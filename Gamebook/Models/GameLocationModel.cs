using System;
using System.Security.Cryptography.X509Certificates;
using Gamebook.Enums;

namespace Gamebook.Models
{
    public class GameLocationModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string BackgroundImage { get; set; }
        public List<TargetLocation> TargetLocations { get; set; }
    }
}

