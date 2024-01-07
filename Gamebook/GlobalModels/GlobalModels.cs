using Gamebook.Enums;
using Gamebook.Models;

namespace Gamebook.GlobalModels;

public static class GlobalModels
{
    public static readonly List<Weapon> Weapons = new()
    {
        new Weapon{Type = WeaponType.Knife, Damage = 20, EnergyConsumption = -10},
        new Weapon{Type = WeaponType.Bat, Damage = 20, EnergyConsumption = -10},
        new Weapon{Type = WeaponType.Gun, Damage = 10, EnergyConsumption = -5},
        new Weapon{Type = WeaponType.Submachine, Damage = 30, EnergyConsumption = -10}
    };

    public static readonly List<Quest> Quests = new()
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

    public static readonly Dictionary<int, GameEnding> Endings = new()
    {
        {1, new() {Title = "Heroic Achievement", Text = "In the dark alleys of corporate power, a ray of light has appeared. Shadow Viper, once a shadow warrior, has become a symbol of courage and truth. His exposure of a corporate scandal not only caused major changes, but inspired a new wave of resistance against corporations. The name Shadow Viper will forever be remembered as an icon of justice."}},
        {2, new(){Title = "The Dark Turn", Text = "In the fight against the corporations, Shadow Viper found himself on the border of light and darkness. His controversial methods and moral compromises led to victory, but at the cost of personal suffering and loss of trust. His name is now a symbol of complex heroism, a reminder that the path to justice is never black and white."}},
        {3, new(){Title = "Sacrifice for the Greater Good", Text = "In his darkest hour, Shadow Viper chose to make the ultimate sacrifice. His death was not in vain; it became the catalyst for a global movement against corporations. The story of Shadow Viper has become legend, an eternal reminder that one brave soul can change the world. His legacy will live on in the hearts of all who dream of freedom and justice."}}
    };
}