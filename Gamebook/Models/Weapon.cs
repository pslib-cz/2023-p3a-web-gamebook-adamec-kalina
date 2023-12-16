using Gamebook.Enums;

namespace Gamebook.Models;

public class Weapon
{
    public WeaponType Type { get; set; }
    public int Demage { get; set; }
    public int EnergyConsumption { get; set; }
}