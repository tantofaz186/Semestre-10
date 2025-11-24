using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class CombatManager : MonoBehaviour
    {
        public ActorUI actorUIPrefab;
        public GridLayoutGroup playerUIGrid;
        public GridLayoutGroup enemyUIGrid;
        private int turn;
        public void Setup(CombatScene scene)
        {
            turn = 0;
            foreach (var actor in scene.players)
            {
                var uiObj = Instantiate(actorUIPrefab, playerUIGrid.transform);

                uiObj.Setup(actor);
            }
            foreach (var actor in scene.enemies)
            {
                var uiObj = Instantiate(actorUIPrefab, enemyUIGrid.transform);
                uiObj.Setup(actor);
            }
        }

        private void Start()
        {
            
        }
    }
}
