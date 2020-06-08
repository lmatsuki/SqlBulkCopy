using System;
using System.Collections.Generic;

namespace Demo.Domain.Entities
{
    public class Player
    {
        public Player()
        {
            Skills = new List<Skill>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public int Hp { get; set; }
        public int Mp { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int MagicAttack { get; set; }
        public int MagicDefense { get; set; }
        public int Level { get; set; }
        public int CorrelationId { get; set; }

        public IList<Skill> Skills { get; set; }
    }
}
