using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextValue : MonoBehaviour
{
    [SerializeField] Slider slider;
    TextMeshProUGUI text;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        ChangeTextValue(slider.value);
        slider.onValueChanged.AddListener(ChangeTextValue);
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(ChangeTextValue);
    }

    private void ChangeTextValue(float val)
    {
        text.text = $"{val * 100:00}";
    }
}
