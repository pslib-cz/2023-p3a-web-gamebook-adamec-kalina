using Gamebook.Enums;

namespace Gamebook.Models;

public class LocationPageResponse
{
    public List<TargetLocation> TargetLocations { get; init; }
    public List<Dialog>? Dialogs { get; set; } = null;
    public PlayerStats PlayerStats { get; set; }
    public Weapon EquipedWeapon { get; set; }
    public HitboxType? Hitbox { get; set; }
}