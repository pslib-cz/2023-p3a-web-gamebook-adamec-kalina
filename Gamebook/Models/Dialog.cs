using Gamebook.Enums;

namespace Gamebook.Models;

public class Dialog
{
    public List<string> Texts { get; set; }
    public bool Available { get; set; } = true; // Dialog available by default
    public PlayerFocus? DialogFocus { get; set; } = null;
    public List<string>? Unlock { get; set; } = null; // A list of string so not a int is passed to the FE
    public List<string>? ItemsGet { get; set; } = null; // A list of string so not a int is passed to the FE
    public List<string>? ItemsRemove { get; set; } = null; // A list of string so not a int is passed to the FE
    public GameProgress DialogOrder { get; set; } // Sets the right game phase for the dialog to be provided
}