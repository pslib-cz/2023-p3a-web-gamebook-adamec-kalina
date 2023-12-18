using Gamebook.Enums;

namespace Gamebook.Interfaces;

public interface IGameplayService
{
    void HealthChange(int health);
    void EnergyChange(int energy);
    void DialogNotAvailable(string currentLocation);
    
}