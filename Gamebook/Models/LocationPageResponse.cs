using Gamebook.Enums;

namespace Gamebook.Models;

public class LocationPageResponse
{
    public List<TargetLocation> TargetLocations { get; init; }
    public Dialog? Dialog { get; set; } = null;
    public PlayerStats PlayerStats { get; set; }
    public Weapon EquipedWeapon { get; set; }
    public string? Hitbox { get; set; }
}