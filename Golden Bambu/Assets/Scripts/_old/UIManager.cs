using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button startButton;
    public Button exitButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(QuitGame);
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
}
