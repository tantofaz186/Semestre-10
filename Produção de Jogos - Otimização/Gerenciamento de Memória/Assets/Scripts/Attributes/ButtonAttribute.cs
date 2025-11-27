using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method)]
public class ButtonAttribute : PropertyAttribute
{
    public readonly string ButtonName;
    public readonly bool IsPlayModeOnly;

    public ButtonAttribute(string buttonName = null, bool isPlayModeOnly = false)
    {
        ButtonName = buttonName;
        IsPlayModeOnly = isPlayModeOnly;
    }
}