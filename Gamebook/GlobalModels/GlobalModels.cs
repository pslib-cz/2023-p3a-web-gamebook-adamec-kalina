using Gamebook.Enums;
using Gamebook.Models;

namespace Gamebook.GlobalModels;

public static class GlobalModels
{
    public static List<Weapon> Weapons = new()
    {
        new Weapon{Type = WeaponType.Knife, Damage = 20, EnergyConsumption = -10},
        new Weapon{Type = WeaponType.Bat, Damage = 20, EnergyConsumption = -10},
        new Weapon{Type = WeaponType.Gun, Damage = 10, EnergyConsumption = -5},
        new Weapon{Type = WeaponType.Submachine, Damage = 30, EnergyConsumption = -10}
    };

    public static List<Quest> Quests = new()
    {
        new Quest{Number = 1, Name = "Lost Memories", Description = "You have awaken in a foreign district and you have no memories.", Task = "Ask around if anyone knows how you got there."},
        new Quest{Number = 2, Name = "", Description = "", Task = ""},
        new Quest{Number = 3, Name = "", Description = "", Task = ""},
        new Quest{Number = 4, Focus = PlayerFocus.Physics, Name = "", Description = "", Task = ""},
        new Quest{Number = 4, Focus = PlayerFocus.Hack, Name = "", Description = "", Task = ""},
        new Quest{Number = 5, Focus = PlayerFocus.Physics, Name = "", Description = "", Task = ""},
        new Quest{Number = 5, Focus = PlayerFocus.Hack, Name = "", Description = "", Task = ""},
        new Quest{Number = 6, Name = "", Description = "", Task = ""},
        new Quest{Number = 7, Name = "", Description = "", Task = ""},
    };
}