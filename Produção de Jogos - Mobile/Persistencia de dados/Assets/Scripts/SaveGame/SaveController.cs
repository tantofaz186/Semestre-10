using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

public static class SaveController
{
    //private static string folderName = "./saves/";
    private static string folderName = Path.Combine(Application.persistentDataPath, "saves");
    private static string fileName = "save";

    public static void SaveSettings(SettingsData settings)
    {
        PlayerPrefs.SetFloat("masterVolume", settings.masterVolume);
        PlayerPrefs.SetFloat("musicVolume", settings.musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", settings.sfxVolume);
        PlayerPrefs.SetFloat("controlSensitivity", settings.controlSensitivity);
        PlayerPrefs.SetString("lastVisitedLevelName", settings.lastVisitedLevelName);
        PlayerPrefs.SetString("lastSelectedSkin", settings.lastSelectedSkin);
        PlayerPrefs.SetInt("totalAccumulatedCoins", settings.totalAccumulatedCoins);
        

        PlayerPrefs.SetInt("vibrationEnabled", settings.vibrationEnabled ? 1 : 0);
    }

    public static SettingsData LoadSettings()
    {

        SettingsData settings = new SettingsData();
        settings.masterVolume = PlayerPrefs.GetFloat("masterVolume", 1.0f);
        settings.musicVolume = PlayerPrefs.GetFloat("musicVolume", 1.0f);
        settings.sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1.0f);
        settings.controlSensitivity = PlayerPrefs.GetFloat("controlSensitivity", 1.0f);
        settings.lastVisitedLevelName = PlayerPrefs.GetString("lastVisitedLevelName", "Level1");
        settings.lastSelectedSkin = PlayerPrefs.GetString("lastSelectedSkin", "Default");
        settings.totalAccumulatedCoins = PlayerPrefs.GetInt("totalAccumulatedCoins", 0);

        settings.vibrationEnabled = PlayerPrefs.GetInt("vibrationEnabled", 1) == 1;
        return settings;
    }

    public static void SaveGame(int saveNumber)
    {
        if (!Directory.Exists(folderName))
        {
            Directory.CreateDirectory(folderName);
        }

        DirectoryInfo dir = new DirectoryInfo(folderName);
        string path = Path.Combine(folderName, fileName + saveNumber + ".xml");

        using (StreamWriter exitfile = new StreamWriter(path))
        {
            XmlSerializer xmlObj = new XmlSerializer(typeof(SaveGame));

            SaveGame newSave = new SaveGame();
            
            List<ISaveable> allSaveables = GameObject.FindObjectsByType<MonoBehaviour>
                    (FindObjectsInactive.Include, FindObjectsSortMode.None)
                .OfType<ISaveable>().ToList();
            foreach (ISaveable saveable in allSaveables)
            {
                newSave.saveData.Add(saveable.Sincronize());
            }
            newSave.initialTime = DateTime.Now;
            xmlObj.Serialize(exitfile.BaseStream, newSave);
            exitfile.Close();
            Debug.Log("Player Saved");
        }

    }

    public static DateTime LoadGame(int saveNumber)
    {
        string path = Path.Combine(folderName, fileName + saveNumber + ".xml");
        if (!File.Exists(path)) return DateTime.MinValue;

        using (StreamReader enterFile = new StreamReader(path))
        {
            XmlSerializer xmlObj = new XmlSerializer(typeof(SaveGame));
            
            SaveGame loaded = (SaveGame)xmlObj.Deserialize(enterFile.BaseStream);


            List<ISaveable> allSaveables = GameObject.FindObjectsByType<MonoBehaviour>
                (FindObjectsInactive.Include, FindObjectsSortMode.None)
                .OfType<ISaveable>().ToList();

            foreach (ISaveable saveable in allSaveables)
            {
                saveable.Load(loaded.saveData.First((data) => 
                    data.GetType() == 
                    saveable.GetType().GetInterfaces()
                    .First((i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISaveable<>)).GetGenericArguments()[0]
                    )
                );
            }


            enterFile.Close();
            Debug.Log("Player Loaded");
            return loaded.initialTime;
        }
    }

    public static void Test()
    {
        Debug.Log("Starting Test");
        List<ISaveable> allSaveables = GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .OfType<ISaveable>().ToList();
        Debug.Log($"Found {allSaveables.Count} MonoBehaviours");

        Debug.Log($"Found {allSaveables.Count} ISaveables");
        foreach (ISaveable saveable in allSaveables)
        {
            Type type = saveable.GetType();
            Type @interface = type.GetInterfaces().First((i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISaveable<>));
            Debug.Log(@interface.GetGenericArguments()[0]);
        }

        Debug.Log("Test Finished");
    }

    public static void DeleteGame(int saveNumber)
    {
        string path = Path.Combine(folderName, fileName + saveNumber + ".xml");
        if (!File.Exists(path)) return;
        File.Delete(path);
    }
}