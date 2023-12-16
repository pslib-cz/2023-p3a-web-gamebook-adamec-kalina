namespace Gamebook.Models;

public class LocationPageResponse
{
    public List<TargetLocation> TargetLocations { get; set; }
    public Dialog Dialog { get; set; }
    public PlayerStats PlayerStats { get; set; }
    public Weapon EquipedWeapon { get; set; }
}