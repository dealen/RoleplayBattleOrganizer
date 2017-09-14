using RoleplayBattleOrganizer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleplayBattleOrganizer.Models
{
    public class FighterGamerType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public FighterType Type { get; set; }
    }

    public class FighterSystemType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public FighterSystem System { get; set; }
    }
}
