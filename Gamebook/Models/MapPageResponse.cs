using Gamebook.Enums;

namespace Gamebook.Models;

public class MapPageResponse
{
    public Location CurrentLocation { get; set; }
    public List<Quest> QuestList { get; set; }
    public FastTravelLocationsModel FastTravelLocations { get; set; }
}