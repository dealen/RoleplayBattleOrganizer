using RoleplayBattleOrganizer.Utility;
using System;

namespace RoleplayBattleOrganizer.Models
{
    public class Fighter
    {
        private int _intMaxHealtPoints;
        private int _intHealthPoints;
        private int _intInitiativeRoll;

        #region Public Constructor

        public Fighter(Guid id,
            int maxHealth, int initiative, string name, string gamer, string prof, FighterSystem system, FighterType type)
        {
            Id = id;
            Name = name;
            Initiative = initiative;
            MaxHealthPoints = maxHealth;
            FighterType = type;
            FighterSystem = system;
            Gamer = gamer;
            Profession = prof;
        }

        public Fighter(Fighter fighter)
        {
            Id = fighter.Id;
            Name = fighter.Name;
            Gamer = fighter.Gamer;
            Initiative = fighter.Initiative;
            MaxHealthPoints = fighter.MaxHealthPoints;
            FighterType = fighter.FighterType;
            FighterSystem = fighter.FighterSystem;
            Profession = fighter.Profession;
            _intInitiativeRoll = fighter.InitiativeRoll;
        }

        public Fighter() { }

        #endregion Public Constructor


        #region Public Properties

        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Initiative { get; set; }

        public int InitiativeRoll
        {
            get { return _intInitiativeRoll; }
        }

        public string Profession { get; set; }

        public FighterType FighterType { get; set; }

        public FighterSystem FighterSystem { get; set; }

        public int HealthPoints
        {
            get { return _intHealthPoints; }
            set { _intHealthPoints = value; }
        }

        public int MaxHealthPoints
        {
            get { return _intMaxHealtPoints; }
            private set
            {
                _intMaxHealtPoints = value;
                HealthPoints = value;
            }
        }

        public string HP
        {
            get { return $"{HealthPoints}/{MaxHealthPoints}"; }
        }

        public string Gamer { get; set; }

        public bool IsAlive
        {
            get { return HealthPoints > 0; }
        }

        public bool IsDamaged
        {
            get { return HealthPoints > 0 && HealthPoints < MaxHealthPoints; }
        }

        public bool IsHeavilyInjured
        {
            get { return HealthPoints >= 0 && HealthPoints < (MaxHealthPoints/3); }
        }

        public string Type
        {
            get { return FighterType == FighterType.Player ? "BG" : "BN"; }
        }

        #endregion Public Properties



        #region Public Methods

        public void SetInitiativeRoll()
        {
            Random rand = new Random();
            int max = 0;
            if (FighterSystem == FighterSystem.D20)
                max = 20;
            else if (FighterSystem == FighterSystem.WHFB)
                max = 10;
            _intInitiativeRoll = Initiative + rand.Next(1, max);
        }

        public override string ToString()
        {
            return $"{InitiativeRoll:00} - {Name} - HP:{HealthPoints}/{MaxHealthPoints}";
        }

        #endregion Public Methods
    }
}
