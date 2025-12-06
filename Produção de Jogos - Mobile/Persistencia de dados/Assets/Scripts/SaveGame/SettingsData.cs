using System;
using System.Collections.Generic;

[Serializable]
public class SettingsData
{
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
    public float controlSensitivity;
    public string lastVisitedLevelName;
    public string lastSelectedSkin;
    public int totalAccumulatedCoins;
    public bool vibrationEnabled;
}