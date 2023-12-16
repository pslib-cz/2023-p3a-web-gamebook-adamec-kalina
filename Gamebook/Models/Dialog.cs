namespace Gamebook.Models;

public class Dialog
{
    public List<string> Texts { get; set; }
    public bool Available { get; set; } = true; // Dialog available by default
}