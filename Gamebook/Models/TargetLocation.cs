using Gamebook.Enums;

namespace Gamebook.Models;

public class TargetLocation
{
    public Location Location { set; get; }
    public bool Locked { get; set; } = false; // Location unlocked by default
}