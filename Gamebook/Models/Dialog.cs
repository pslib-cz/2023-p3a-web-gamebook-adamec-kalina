namespace Gamebook.Models;

public class Dialog
{
    public string Text { get; set; }
    public bool Available { get; set; } = true; // Dialog available by default
}