using System;
using Obstacles;

namespace Collectables
{
    public class Star : Collectable
    {
        public static event Action<float> OnStarCollected;

        protected override void OnPlayerCollision()
        {
            OnStarCollected?.Invoke(3f);
        }
        public override float chanceToSpawn => 0.05f;
    }
}