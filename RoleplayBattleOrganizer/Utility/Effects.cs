using RoleplayBattleOrganizer.Models;
using RoleplayBattleOrganizer.Utility.Interfaces;
using System;

namespace RoleplayBattleOrganizer.Utility
{
    public class Effects : IEffects
    {
        public Fighter Fighter { get; private set; }

        public Effects(Fighter _fighter)
        {
            this.Fighter = _fighter;
        }

        public void AddDamage(int damage)
        {
            if (Fighter.HealthPoints >= -10)
                Fighter.HealthPoints -= damage;
        }

        public void AddHealing(int healing)
        {
            Fighter.HealthPoints += healing;
            if (Fighter.HealthPoints > Fighter.MaxHealthPoints)
                Fighter.HealthPoints = Fighter.MaxHealthPoints;
        }

        public void AddTemporaryHealthPoints(int hp)
        {
            throw new NotImplementedException();
        }
    }
}
