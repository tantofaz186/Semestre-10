using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WeakPoint : MonoBehaviour
{

    public Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    public bool Contains(Ray cut, float rayLength)
    {
        Debug.Log(
            $"Checking if {name} contains cut ray from {cut.origin} to {cut.direction} - {GetComponent<Collider>().bounds.IntersectRay(cut)}");
        bool r = GetComponent<Collider>().bounds.IntersectRay(cut, out float hitLenght)
               && hitLenght <= rayLength;
        Debug.Log($"{hitLenght} ---- {rayLength}");
        return r;
    }
}