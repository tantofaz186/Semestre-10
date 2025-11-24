using System.Collections.Generic;

namespace Combat
{
    public class CombatScene
    {
        List<Actor> actors;
        
        public CombatScene(List<Actor> actors)
        {
            this.actors = actors;
        }
    }
}