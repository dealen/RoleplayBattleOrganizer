using RoleplayBattleOrganizer.Models;

namespace RoleplayBattleOrganizer.Utility.Interfaces
{
    interface IEffects
    {
        void AddDamage(int damage);
        void AddHealing(int healing);
        void AddTemporaryHealthPoints(int hp);
    }
}
