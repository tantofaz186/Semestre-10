using TMPro;
using UnityEngine;

public class PointsText : MonoBehaviour
{
    private TextMeshProUGUI pointsText;

    private void Awake()
    {
        pointsText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateScore()
    {
        pointsText.text = CuttableObject.points.ToString();
    }
}
