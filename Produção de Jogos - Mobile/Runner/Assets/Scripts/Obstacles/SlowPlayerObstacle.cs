using Managers;

namespace Obstacles
{
    public class SlowPlayerObstacle : Obstacle
    {
        protected override void OnPlayerCollision()
        {
            GameManager.Instance.ApplyPlayerSlow();
        }
    }
}