using UnityEngine;

public class Slow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Time.timeScale = 0.2f;
    }

}
