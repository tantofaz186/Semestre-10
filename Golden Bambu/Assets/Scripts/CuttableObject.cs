using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CuttableObject : MonoBehaviour
{
    public List<WeakPointPair> weakPoints;
    private IEnumerable<WeakPointPair> UncutWeakPoints => weakPoints.Where((w) => !w.cut);

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
        foreach (WeakPointPair weakPointPair in UncutWeakPoints)
        {
            if (weakPointPair.TouchedPair(start, end))
            {
                weakPointPair.Cut();
                break;
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
    public bool cut { get; private set; }

    public bool TouchedPair(Vector3 start, Vector3 end)
    {
        Vector3 origin = start;
        Vector3 direction = end;

        origin.z = first.transform.position.z;
        direction.z = last.transform.position.z;

        direction -= origin;
        Ray ray = new Ray(origin, direction);

        // return first.Intersects(Sword.Instance.lineRenderer.bounds) 
        //        && last.Intersects(Sword.Instance.lineRenderer.bounds);
        bool r = first.Contains(ray, direction.magnitude)
               && last.Contains(ray, direction.magnitude);
        if (r)
        {
            Ray lineRay = new Ray(start, end - start);
            Sword.Instance.SetLine(ray, direction.magnitude);
        }

        return r;
    }

    public void SetSwordLine(Ray ray)
    {
        Sword.Instance.SetLine(ray, ray.direction.magnitude);
    }

    public void Cut()
    {
        first.gameObject.SetActive(false);
        last.gameObject.SetActive(false);
        cut = true;
    }

    public void Reset()
    {
        first.gameObject.SetActive(true);
        last.gameObject.SetActive(true);
        cut = false;
    }
}