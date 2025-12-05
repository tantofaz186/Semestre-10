using System;
using System.Collections.Generic;

[System.Serializable]
public class SaveGame
{
    public List<SaveData> saveData;
    public DateTime initialTime;
    
    public SaveGame()
    {
        saveData = new List<SaveData>();
        initialTime = DateTime.Now;
    }
}
