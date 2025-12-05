using UnityEngine;
using TMPro;
using System;

public class UIController : MonoBehaviour
{
    public TMP_Text timeText;
    public void SaveGame(int saveNumber)
    {
        SaveController.SaveGame(saveNumber);
    }

    public void LoadGame(int saveNumber)
    {
        DateTime time = SaveController.LoadGame(saveNumber);
        if (time == DateTime.MinValue) return;
        timeText.text = time.ToString();
    }

    public void DeleteGame(int saveNumber)
    {
        SaveController.DeleteGame(saveNumber);
    }

    public void Test()
    {
        SaveController.Test();
    }
}
