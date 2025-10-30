using TMPro;
using UnityEngine;

public class PointsText : MonoBehaviour
{
    private TextMeshProUGUI pointsText;

    private void Awake()
    {
        pointsText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        UpdateScore();
    }

    public void UpdateScore()
    {
        pointsText.text = CutManager.Instance.points.ToString();
    }
}
