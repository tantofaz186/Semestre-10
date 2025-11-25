using UnityEngine;

public class SampleModule : MonoBehaviour
{
    public Transform[] spawnPoints;
    public bool[] occupied;
    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            spawnPoints = GetComponentsInChildren<Transform>()[1..];
        }

        if (occupied == null || occupied.Length != spawnPoints.Length)
        {
            occupied = new bool[spawnPoints.Length];
        }
    }
    #endif
}