using System;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public void TriggerWin()
    {
        GameManager.Instance.FireWinEvent();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerWin();
        }
    }
}
