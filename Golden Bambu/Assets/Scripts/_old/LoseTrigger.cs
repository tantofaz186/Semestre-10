using UnityEngine;

public class LoseTrigger : MonoBehaviour
{

    public void TriggerLose()
    {
        GameManager.Instance.FireLoseEvent();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerLose();
        }
    }
}
