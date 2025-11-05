using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public GameObject settings;
    public Button startGameButton;
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
        startGameButton.gameObject.SetActive(true);
        settings.SetActive(false);
    }
    public void ToggleSettings()
    {
        startGameButton.gameObject.SetActive(settings.activeSelf);
        settings.SetActive(!settings.activeSelf);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
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
