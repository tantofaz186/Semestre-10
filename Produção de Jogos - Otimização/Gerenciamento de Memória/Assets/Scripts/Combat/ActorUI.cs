using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class ActorUI : MonoBehaviour
    {
        public Actor actor;
        public Sprite AttackSprite;
        public Sprite DefendSprite;
        public Sprite HealSprite;
        public Slider slider;
        public void Setup(Actor actor)
        {
            GetComponent<Image>().sprite = actor.icon;
            slider.maxValue = actor.stats.maxHealth;
            slider.value = actor.stats.health;
            foreach (Image image in GetComponentsInChildren<Image>())
            {
                switch (image.name)
                {
                    case "Attack":
                        image.sprite = AttackSprite;
                        break;
                    case "Defend":
                        image.sprite = DefendSprite;
                        break;
                    case "Heal":
                        image.sprite = HealSprite;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    [CustomEditor(typeof(ActorUI))]
    public class ActorUIEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ActorUI actorUI = (ActorUI)target;

            if(GUILayout.Button("Setup"))
            {
                actorUI.Setup(actorUI.actor);
            }
        }
    }
}