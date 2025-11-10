using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public GameObject settings;
    public GameObject mainMenu;
    public GameObject scorePanel;
    public TextMeshProUGUI maxScoreTextValue;
    public TextMeshProUGUI lastScoreTextValue;
    void Awake()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        }
        else
        {
            masterVolumeSlider.value = musicVolumeSlider.value = sfxVolumeSlider.value = 1;
        }

        SetupUI();
    }

    private void SetupUI()
    {
        scorePanel.SetActive(PlayerPrefs.HasKey("MaxScore"));
        maxScoreTextValue.text = $"{PlayerPrefs.GetInt("MaxScore", 0): 00000}";
        lastScoreTextValue.text = $"{PlayerPrefs.GetInt("FinalScore", 0): 00000}";
        mainMenu.SetActive(true);
        settings.SetActive(false);
    }
    public void ToggleSettings()
    {
        mainMenu.SetActive(settings.activeSelf);
        settings.SetActive(!settings.activeSelf);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("TestScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void ChangeMasterVolume(float vol)
    {
        PlayerPrefs.SetFloat("MasterVolume", vol);
    }
    public void ChangeMusicVolume(float vol)
    {
        PlayerPrefs.SetFloat("MusicVolume", vol);
    }
    public void ChangeSFXVolume(float vol)
    {
        PlayerPrefs.SetFloat("SFXVolume", vol);
    }
}
