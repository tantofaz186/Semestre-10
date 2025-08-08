using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WeakPoint : MonoBehaviour
{
    public bool Contains(Ray cut, float rayLength)
    {
        Debug.Log(
            $"Checking if {name} contains cut ray from {cut.origin} to {cut.direction} - {GetComponent<Collider>().bounds.IntersectRay(cut)}");

        return GetComponent<Collider>().bounds.IntersectRay(cut, out float hitLenght)
               && hitLenght <= rayLength;
    }
}