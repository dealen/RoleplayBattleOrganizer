using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoleplayBattleOrganizer.Models;
using RoleplayBattleOrganizer.Utility;
using System;

namespace TestRoleplayBattleOrganizer
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CreatingFighter()
        {
            Fighter fighter = new Fighter(Guid.NewGuid(),12, 12, "Kuba", "Grori", "Runepriest", FighterSystem.D20, FighterType.Player);
            Assert.IsNotNull(fighter);
        }

        [TestMethod]
        public void DamagingFighter()
        {
            Fighter fighter = new Fighter(Guid.NewGuid(), 12, 12, "Kuba", "Grori", "Runepriest", FighterSystem.D20, FighterType.Player);
            Effects ef = new Effects(fighter);
            ef.AddDamage(2);
            Assert.AreNotSame(fighter.MaxHealthPoints, fighter.HealthPoints);
        }

        [TestMethod]
        public void HealingFighter()
        {
            Fighter fighter = new Fighter(Guid.NewGuid(), 12, 12, "Kuba", "Grori", "Runepriest", FighterSystem.D20, FighterType.Player);
            Effects ef = new Effects(fighter);
            ef.AddDamage(3);
            ef.AddHealing(5);
            Assert.AreNotEqual(12 - 4, 12 - 2);
        }

        [TestMethod]
        public void IsAlive()
        {
            Fighter fighter = new Fighter(Guid.NewGuid(), 12, 12, "Kuba", "Grori", "Runepriest", FighterSystem.D20, FighterType.Player);
            Effects ef = new Effects(fighter);
            ef.AddDamage(11);
            Assert.IsTrue(fighter.IsAlive);
        }
        
    }
}
