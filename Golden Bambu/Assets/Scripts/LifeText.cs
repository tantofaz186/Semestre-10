using System;
using System.Text;
using TMPro;
using UnityEngine;

public class LifeText : MonoBehaviour
{
    private TextMeshProUGUI pointsText;
    
    // ● ○
    private char circleSymbol = '\u25CF';
    private char emptyCircleSymbol = '\u25cb';

    private byte maxLivesDisplayed = 7;
    private void Awake()
    {
        pointsText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(UpdateLifeDisplay), 0f, 0.5f);
    }

    public void UpdateLifeDisplay()
    {
        int currentLives = (int)LoseLifeOnTrigger.instance.Life;
        pointsText.text = new StringBuilder().Append(circleSymbol, currentLives)
            .Append(emptyCircleSymbol, (maxLivesDisplayed - Math.Min(currentLives, maxLivesDisplayed)))
            .ToString();
    }

}
