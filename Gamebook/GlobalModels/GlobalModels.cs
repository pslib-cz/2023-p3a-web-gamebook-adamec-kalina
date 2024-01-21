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
        new Quest{Number = 2, Name = "First Touch", Description = "Old Merchant told you about the hackers, he spotted near his shop.", Task = "Word on the street is you'll find them at the Shady Bar."},
        new Quest{Number = 3, Name = "Cyberware", Description = "You've made it into the group, prepare for incoming tasks.", Task = "Get some upgrades from Ripper Dock."},
        new Quest{Number = 4, Focus = PlayerFocus.Physics, Name = "Rescue Mission", Description = "You have chosen the SKELLETRON. BIG SCARY DUDE!", Task = "Rescue your colleague, be carefull of security."},
        new Quest{Number = 4, Focus = PlayerFocus.Hack, Name = "Digital Infiltration", Description = "You have chosen the BRAIN CHIP. NERD!", Task = "Hack the corporate database."},
        new Quest{Number = 5, Focus = PlayerFocus.Physics, Name = "Sabotage", Description = "The corporation is about to launch a new project. Don't let it.", Task = "Stop the corporation from releasing it into the world."},
        new Quest{Number = 5, Focus = PlayerFocus.Hack, Name = "Information War", Description = "Our networks are facing coordinated cyber attacks.", Task = "Return the favour and STOP THEM!"},
        new Quest{Number = 6, Name = "Cyber Infiltration", Description = "This task can be done only from the inside.", Task = "Go in and hack them, easy right ;)"},
        new Quest{Number = 7, Name = "Decisive Confrontation", Description = "STOP THE CORPORATION ONE AND FOR ALL", Task = "MAKE THEM BLEED!"},
    };

    public static readonly Dictionary<int, GameEnding> Endings = new()
    {
        {1, new() {Title = "Heroic Achievement", Text = "In the dark alleys of corporate power, a ray of light has appeared. Shadow Viper, once a shadow warrior, has become a symbol of courage and truth. His exposure of a corporate scandal not only caused major changes, but inspired a new wave of resistance against corporations. The name Shadow Viper will forever be remembered as an icon of justice."}},
        {2, new(){Title = "The Dark Turn", Text = "In the fight against the corporations, Shadow Viper found himself on the border of light and darkness. His controversial methods and moral compromises led to victory, but at the cost of personal suffering and loss of trust. His name is now a symbol of complex heroism, a reminder that the path to justice is never black and white."}}
    };
}