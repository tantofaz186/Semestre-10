using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public static GameManager Instance => instance;

    public event Action OnLose;
    public event Action OnWin;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    public void FireWinEvent()
    {
        OnWin?.Invoke();
    }

    public void FireLoseEvent()
    {
        OnLose?.Invoke();
    }
}
