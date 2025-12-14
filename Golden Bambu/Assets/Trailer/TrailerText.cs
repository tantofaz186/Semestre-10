using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TrailerText : MonoBehaviour
{
    private static string[] texts = {
        "CORRA",
        "CORTE",
        "EXECUTE"
    };
    public TextMeshProUGUI text;
    public int minFontSize;
    public int maxFontSize;

    private int index = 0;
    private int Index
    {
        get { return index % 3; }
        set { index = value % 3; }
    }
    public int lerp;
    public float acceptableEpsilon = 0.25f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text =  GetComponent<TextMeshProUGUI>();
        text.fontSize = minFontSize;
        StartCoroutine(ChangeText());
    }

    private IEnumerator ChangeText()
    {
        text.text = texts[Index++];
        yield return new WaitUntil(() =>
        {
            text.fontSize = Mathf.Lerp(text.fontSize, maxFontSize, lerp* Time.deltaTime);
            return text.fontSize + acceptableEpsilon >= maxFontSize;
        });
        text.fontSize = minFontSize;
        StartCoroutine(ChangeText());
    }
}
