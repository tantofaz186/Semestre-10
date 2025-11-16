using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseLifeOnTrigger : MonoBehaviour
{
    [SerializeField] private uint life = 1;
    public uint Life => life;

    public static LoseLifeOnTrigger instance;

    private void Awake()
    {
        life = 999;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        LoseLife();
    }

    public void GainLife()
    {
        life++;
    }

    public void LoseLife()
    {
        life--;
        if (life <= 0) TriggerGameOver();
        CutManager.Instance.CutAllObjects();
    }

    private void TriggerGameOver()
    {
        PlayerPrefs.SetInt("FinalScore", (int)CutManager.Instance.points);
        PlayerPrefs.SetInt("MaxScore", Mathf.Max(PlayerPrefs.GetInt("MaxScore"), (int)CutManager.Instance.points));
        SceneManager.LoadScene("MenuScene");
    }
}