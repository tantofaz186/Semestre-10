using TMPro;
using UnityEngine;

namespace Managers
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ScoreManager : MonoBehaviour
    {
        public uint Score => score;
        [SerializeField] private uint score;
        TextMeshProUGUI scoreText;
        private void Awake()
        {
            scoreText = GetComponent<TextMeshProUGUI>();
        }
        private void FixedUpdate()
        {
            score = (uint)GameManager.Instance.player.transform.position.z;
            scoreText.text = score.ToString();
        }
    }
}
