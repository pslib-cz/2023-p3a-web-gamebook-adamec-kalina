using Gamebook.Enums;

namespace Gamebook.Interfaces;

public interface IGameplayService
{
    void HealthChange(int health);
    void EnergyChange(int energy);
    public void MoneyChange(int change);
    void SetPlayerFocusChoice(PlayerFocus playerFocus);
    void SetDialogNotAvailable();
    public void UnlockLocation(Location locationString);
}