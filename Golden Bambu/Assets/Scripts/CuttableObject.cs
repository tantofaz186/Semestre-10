using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CuttableObject : MonoBehaviour
{
    public List<WeakPointPair> weakPoints;

    private void Start()
    {
        Sword.Instance.OnSwipeEnd += OnSwipeEnd;
    }

    private void OnDisable()
    {
        Sword.Instance.OnSwipeEnd -= OnSwipeEnd;
    }

    private void OnSwipeEnd(Vector3 start, Vector3 end)
    {
        foreach (WeakPointPair weakPointPair in weakPoints)
        {
            if (weakPointPair.TouchedPair(start, end))
            {
                weakPointPair.cut = true;
            }
        }

        if (weakPoints.All((w) => w.cut))
        {
            Destroy(gameObject);
        }
    }
}

[System.Serializable]
public class WeakPointPair
{
    public WeakPoint first;
    public WeakPoint last;
    public bool cut;
    public bool TouchedPair(Vector3 start, Vector3 end)
    {
        Vector3 origin = start;
        Vector3 direction = end;
        
        origin.z = first.transform.position.z;
        direction.z = last.transform.position.z;
        
        direction -= origin; // Calculate the direction vector
        Ray ray = new Ray(origin, direction);
        return first.Contains(ray) && last.Contains(ray);

    }
}