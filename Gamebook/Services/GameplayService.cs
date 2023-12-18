using Gamebook.Interfaces;
using Gamebook.Pages;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Gamebook.Services;

public class GameplayService //:IGameplayService
{
    private readonly ISessionHelper _session;
    private readonly IGameLocationService _locationService;

    public GameplayService(ISessionHelper session, IGameLocationService locationService)
    {
        _session = session;
        _locationService = locationService;
    }
    
    public void HealthChange(int health)
    {
        
        //TODO check if lower than 0 -> call method for redirecting on a deathScreen
        //else
        //TODO set health in session
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