using Gamebook.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamebook.Services;

public class GameplayService : IGameplayService
{
    public void HealthChange(int health)
    {
        throw new NotImplementedException();
    }

    public void EnergyChange(int energy)
    {
        throw new NotImplementedException();
    }

    public void DialogNotAvailable(string currentLocation)
    {
        throw new NotImplementedException();
    }
}