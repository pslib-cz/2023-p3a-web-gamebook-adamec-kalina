using Gamebook.Enums;

namespace Gamebook.Models;

public class Hitbox
{
    public HitboxType Type { get; set; }
    public bool Available { get; set; } = true;
}