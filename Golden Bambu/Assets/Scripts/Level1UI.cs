using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level1UI : MonoBehaviour
{

    public GameObject WinPanel;
    public GameObject LosePanel;
    public Button restartButton;
    public Button quitToMenuButton;
    public Button nextLevelButton;
    private void Awake()
    {
        restartButton.onClick.AddListener(RestartLevel1);
        quitToMenuButton.onClick.AddListener(LoadMenu);
        nextLevelButton.onClick.AddListener(LoadLevel2);
        LosePanel.SetActive(false);
        WinPanel.SetActive(false);
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
        restartButton.onClick.RemoveListener(RestartLevel1);
        quitToMenuButton.onClick.RemoveListener(LoadMenu);
        nextLevelButton.onClick.RemoveListener(LoadLevel2);
        GameManager.Instance.OnLose -= OnLose;
        GameManager.Instance.OnWin -= OnWin;
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void RestartLevel1()
    {
        SceneManager.LoadScene("Level 1");
    }
}
