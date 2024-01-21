using System.ComponentModel.DataAnnotations;
using Gamebook.Enums;

namespace Gamebook.Models;

public class TargetLocation
{
    public Location Location { get; set; }
    public bool Locked { get; set; }
}