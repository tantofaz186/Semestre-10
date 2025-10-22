using System;
using Collectables;
using TMPro;
using UnityEngine;

namespace Managers
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ScoreManager : MonoBehaviour
    {
        public uint Score => score + modifiers;
        [SerializeField] private uint score;
        TextMeshProUGUI scoreText;
        private uint modifiers;

        private void Start()
        {
            score = 0;
            modifiers = 0;
            PointsUp.OnPointsUpCollected += AddScoreModifier;
        }

        private void OnDestroy()
        {
            PointsUp.OnPointsUpCollected -= AddScoreModifier;
        }

        private void AddScoreModifier(uint mod)
        {
            modifiers += mod;
        }
        private void Awake()
        {
            scoreText = GetComponent<TextMeshProUGUI>();
        }
        private void FixedUpdate()
        {
            score = (uint)GameManager.Instance.player.transform.position.z;
            scoreText.text = Score.ToString();
        }
    }
}
