using System;
using Obstacles;

namespace Collectables
{
    public class Cloud : Collectable
    {
        public static event Action OnCloudCollected;

        protected override void OnPlayerCollision()
        {
            OnCloudCollected?.Invoke();
        }

        public override float chanceToSpawn => 0.1f;
    }
}