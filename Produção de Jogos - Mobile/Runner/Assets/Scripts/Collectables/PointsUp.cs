using System;
using Obstacles;

namespace Collectables
{
    public class PointsUp : Collectable
    {
        public static event Action<uint> OnPointsUpCollected;
        protected override void OnPlayerCollision()
        {
            OnPointsUpCollected?.Invoke(50);
        }

        public override float chanceToSpawn => 0.5f;
    }
}