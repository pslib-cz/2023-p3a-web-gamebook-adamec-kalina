using Gamebook.Enums;

namespace Gamebook.Models;

public class Hitbox
{
    public HitboxType Type { get; set; }
    // public bool Available { get; set; } = true;
    public GameProgress? HitboxOrder { get; set; } 
    public PlayerDealingType? PlayerDealingType { get; set; } = null;
    public PlayerFocus? PlayerFocus { get; set; } = null;


}