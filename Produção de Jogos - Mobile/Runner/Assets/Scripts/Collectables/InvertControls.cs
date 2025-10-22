using System;
using Obstacles;

namespace Collectables
{
    public class InvertControls : Collectable
    {
        public static event Action<float> OnInvertControlsCollected;
        protected override void OnPlayerCollision()
        {
            OnInvertControlsCollected?.Invoke(3f);
        }

        public override float chanceToSpawn => 0.05f;
    }
}