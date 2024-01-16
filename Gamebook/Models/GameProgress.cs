namespace Gamebook.Models;

public class GameProgress
{
    public int Quest { get; set; }
    public int Step { get; set; }
    public bool LastStep { get; set; } = false;
}