using Demo.Domain.Entities;
using System.Collections.Generic;

namespace Demo.Infrastructure.Extensions
{
    public static class InsertExtensions
    {
        public static IList<Player> GetPlayers(int playerCount)
        {
            IList<Player> players = new List<Player>();

            for (int i = 0; i < playerCount; i++)
            {
                int id = i + 1;
                players.Add(new Player
                {
                    Name = $"Player {id}",
                    Skills = new List<Skill>
                    {
                        new Skill(id, "Bash", id),
                        new Skill(id, "Increase Attack", id),
                        new Skill(id, "Heal", id)
                    },
                    CorrelationId = id
                });
            }

            return players;
        }
    }
}
