using Gamebook.Enums;

namespace Gamebook.Models;

public class LoadOutPageResponse
{
    public Location CurrentLocation { get; set; }
    public PlayerFocus? PlayerFocus { get; set; }
    public WeaponType Weapon { get; set; }
    public PlayerStats PlayerStats { get; set; }
    public List<Item> InventoryItems { get; set; }
}