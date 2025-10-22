using Obstacles;

namespace Collectables
{
    public class Coin : Collectable
    {
        public static event CoinEventHandler OnCoinCollected;
        
        protected override void OnPlayerCollision()
        {
            OnCoinCollected?.Invoke(this);
        }

        public override float chanceToSpawn => 0.8f;
    }
    public delegate void CoinEventHandler(Coin coin);

}