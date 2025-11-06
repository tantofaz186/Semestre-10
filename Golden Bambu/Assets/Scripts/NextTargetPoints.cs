using TMPro;
using UnityEngine;

public class NextTargetPointsText : MonoBehaviour
{
    private TextMeshProUGUI pointsText;
    uint nextTargetPoints = 2;
    private void Awake()
    {
        pointsText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(UpdateScore), 0f, 0.5f);
    }

    public void UpdateScore()
    {
        if(CutManager.Instance.points >= nextTargetPoints)
        {
            nextTargetPoints *= 2;
            LoseLifeOnTrigger.instance.GainLife();
        }
        pointsText.text = nextTargetPoints.ToString();
    }
}