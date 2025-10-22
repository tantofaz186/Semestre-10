using Obstacles;

namespace Collectables
{
    public abstract class Collectable : Obstacle
    {
        public abstract float chanceToSpawn { get;  }
    }
}