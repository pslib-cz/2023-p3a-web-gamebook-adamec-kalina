using Gamebook.Enums;

namespace Gamebook.Models;

public class Dialog
{
    public List<string> Texts { get; set; }
    public bool Available { get; set; } = true; // Dialog available by default
    public PlayerFocus? DialogFocus { get; set; } = null;
    public List<string>? Unlock { get; set; } = null; // A list of string so not a int is passed to the FE
}