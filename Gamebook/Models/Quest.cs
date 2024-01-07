using Gamebook.Enums;

namespace Gamebook.Models;

public class Quest
{
    public int Number { get; set; }
    public PlayerFocus? Focus { get; set; } = null;
    public bool Completed { get; set; } = false;
    public string Name { get; set; }
    public string Description { get; set; }
    public string Task { get; set; }

}