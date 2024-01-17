using Gamebook.Enums;

namespace Gamebook.Interfaces;

public interface IGameplayService
{
    void HealthChange(int health);
    void EnergyChange(int energy);
    void MoneyChange(int change);
    void MoralScoreChange(int changeAmount);
    void SetPlayerFocusChoice(PlayerFocus playerFocus);
    void DialogOver();
    void SetHitboxNotAvailable();
    void UnlockLocation(Location locationString);
    void LockLocation(Location locationString);
    void UnlockNextQuest(int questCompletedNum);
    void EquipWeapon(WeaponType type);
    void SetQuestCompleted(int number);
    void AddNewQuest(int number);
    void AddItem(Item item);
    void RemoveItem(Item item);
}