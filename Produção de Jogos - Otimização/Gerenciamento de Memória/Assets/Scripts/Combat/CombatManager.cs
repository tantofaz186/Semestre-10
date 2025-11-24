using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class CombatManager : MonoBehaviour
    {
        public ActorUI PlayerUIPrefab;
        public ActorUI EnemyUIPrefbab;
        public GridLayoutGroup playerUIGrid;
        public GridLayoutGroup enemyUIGrid;
        private int turn;
        public void Setup(CombatScene scene)
        {
            turn = 0;
            foreach (var actor in scene.players)
            {
                var uiObj = Instantiate(PlayerUIPrefab, playerUIGrid.transform);

                uiObj.Setup(actor);
            }
            foreach (var actor in scene.enemies)
            {
                var uiObj = Instantiate(EnemyUIPrefbab, enemyUIGrid.transform);
                uiObj.Setup(actor);
            }
        }

        private void Start()
        {
            
        }
    }
}
