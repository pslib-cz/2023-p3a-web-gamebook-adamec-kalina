using Gamebook.Enums;

namespace Gamebook.Models;

public class LoadOutPageResponse
{
    public Location CurrentLocation { get; set; }
    public PlayerFocus? PlayerFocus { get; set; }
}