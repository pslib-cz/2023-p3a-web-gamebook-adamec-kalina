using Gamebook.Enums;

namespace Gamebook.Interfaces;

public interface IGameplayService
{
    void HealthChange(int health);
    void EnergyChange(int energy);
    void MoneyChange(int change);
    void SetPlayerFocusChoice(PlayerFocus playerFocus);
    void SetDialogNotAvailable();
    void UnlockLocation(Location locationString);
    void EquipWeapon(WeaponType type);
    void SetQuestCompleted(int number);
    void NewQuest(int number);



}