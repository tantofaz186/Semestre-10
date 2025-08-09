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
    public bool Intersects(Bounds bounds)
    {
        return col.bounds.Intersects(bounds);
    }
    public bool Contains(Ray cut, float rayLength)
    {
        bool r = col.bounds.IntersectRay(cut, out float hitLenght)
               && hitLenght <= rayLength;
        return r;
    }
}