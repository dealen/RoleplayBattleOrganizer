using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleplayBattleOrganizer.Models
{
    public class FighterBase
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Gamer { get; set; }

        public int Initiative { get; set; }
        
        public string Profession { get; set; }

        public int FighterType { get; set; }

        public int FighterSystem { get; set; }

        public int HealthPoints { get; set; }

        public int MaxHealthPoints { get; set; }
    }
}
