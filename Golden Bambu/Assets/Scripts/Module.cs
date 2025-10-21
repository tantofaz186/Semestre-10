using System;
using UnityEngine;

public class Module : MonoBehaviour
{
    public Transform[] spawnPoints;
    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            spawnPoints = GetComponentsInChildren<Transform>()[1..];
        }
    }
    #endif
}
