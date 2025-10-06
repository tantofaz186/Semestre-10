using Managers;

namespace Obstacles
{
    public class GameOverObstacle : Obstacle
    {
        protected override void OnPlayerCollision()
        {
            GameManager.Instance.RestartGame();
        }
    }
}