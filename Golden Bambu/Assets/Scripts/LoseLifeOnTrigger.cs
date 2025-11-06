using UnityEngine;

public class LoseLifeOnTrigger : MonoBehaviour
{
    [SerializeField]
    private uint life = 3;
    public uint Life => life;

    public static LoseLifeOnTrigger instance;
    
    private void Awake()
    {
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
    }

    private void TriggerGameOver()
    {
        Debug.Log("Game over");
    }
}