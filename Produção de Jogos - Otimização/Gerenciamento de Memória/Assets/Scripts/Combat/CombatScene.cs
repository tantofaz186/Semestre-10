using System.Collections.Generic;

namespace Combat
{
    public class CombatScene
    {
        public readonly List<Actor> players;
        public readonly List<Actor> enemies;
        public readonly List<Actor> actors;
        
        public CombatScene(List<Actor> players, List<Actor> enemies)
        {
            this.players = players;
            this.enemies = enemies;
            actors = new List<Actor>(players.Count + enemies.Count);
            actors.AddRange(players);
            actors.AddRange(enemies);
            actors.Sort((a, b) => b.stats.speed.CompareTo(a.stats.speed));
        }
    }
}