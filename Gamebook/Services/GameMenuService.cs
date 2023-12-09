using Gamebook.Interfaces;

namespace Gamebook.Services;

public class GameMenuService : IGameMenuService
{
    private readonly ISessionHelper _session;

    public GameMenuService(ISessionHelper session)
    {
        _session = session;
    }
}