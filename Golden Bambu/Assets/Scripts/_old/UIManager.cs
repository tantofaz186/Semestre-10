using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button startButton;
    public Button exitButton;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    void Awake()
    {
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(QuitGame);
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
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    private void QuitGame()
    {
        Application.Quit();
    }
    private void OnDisable()
    {
        startButton.onClick.RemoveListener(StartGame);
        exitButton.onClick.RemoveListener(QuitGame);
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
