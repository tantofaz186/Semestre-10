using System;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    public static event Action OnPlayerPassed;

    private void OnTriggerEnter(Collider other)
    {
        OnPlayerPassed?.Invoke();
    }
}