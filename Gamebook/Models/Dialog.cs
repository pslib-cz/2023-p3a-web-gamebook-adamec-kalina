using Gamebook.Enums;

namespace Gamebook.Models;

public class Dialog
{
    public List<string> Texts { get; set; }
    public PlayerFocus? DialogFocus { get; set; } = null;
    public PlayerDealingType? SpecialType { get; set; } = null;
    public List<Location>? Unlock { get; set; } = null; // A list of string so not a int is passed to the FE
    public List<Item>? ItemsAdd { get; set; } = null; // A list of string so not a int is passed to the FE
    public List<Item>? ItemsRemove { get; set; } = null; // A list of string so not a int is passed to the FE
    public GameProgress DialogOrder { get; set; } // Sets the right game phase for the dialog to be provided
}