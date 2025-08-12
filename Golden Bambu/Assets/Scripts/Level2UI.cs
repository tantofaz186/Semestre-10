using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level2UI : MonoBehaviour
{

    public GameObject WinPanel;
    public GameObject LosePanel;
    public Button restartButton;
    public Button quitToMenuButton;
    public Button quitToMenuButton2;
    public Button quitGameButton;
    private void Awake()
    {
        restartButton.onClick.AddListener(RestartLevel2);
        quitToMenuButton.onClick.AddListener(LoadMenu);
        quitToMenuButton2.onClick.AddListener(LoadMenu);
        quitGameButton.onClick.AddListener(QuitGame);
        LosePanel.SetActive(false);
        WinPanel.SetActive(false);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void Start()
    {
        GameManager.Instance.OnLose += OnLose;
        GameManager.Instance.OnWin += OnWin;
    }

    private void OnLose()
    {
        LosePanel.SetActive(true);
    }
    private void OnWin()
    {
        WinPanel.SetActive(true);
    }

    public void OnDisable()
    {
        restartButton.onClick.RemoveListener(RestartLevel2);
        quitToMenuButton.onClick.RemoveListener(LoadMenu);
        quitToMenuButton2.onClick.AddListener(LoadMenu);
        quitGameButton.onClick.RemoveListener(QuitGame);
        GameManager.Instance.OnLose -= OnLose;
        GameManager.Instance.OnWin -= OnWin;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void RestartLevel2()
    {
        SceneManager.LoadScene("Level 2");
    }
}
