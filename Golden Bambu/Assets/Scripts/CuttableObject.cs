using System.Collections;
using EzySlice;
using UnityEngine;
using Plane = UnityEngine.Plane;
using Random = UnityEngine.Random;

public class CuttableObject : MonoBehaviour
{
    public Rigidbody rb;
    public Material mat;

    private void Start()
    {
        Sword.Instance.OnCut += OnCut;
    }

    public void Reset()
    {
        gameObject.SetActive(true);
        Sword.Instance.OnCut += OnCut;
    }
    
    public void Deactivate()
    {
        Sword.Instance.OnCut -= OnCut;
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }
    
    private void OnDisable()
    {
        Sword.Instance.OnCut -= OnCut;
    }

    private void OnCut(Plane cuttingPlane)
    {
        Vector3 closestPoint = cuttingPlane.ClosestPointOnPlane(transform.position);
        if(Vector3.Distance(closestPoint, transform.position) > 1.5f)
            return;
        StartCoroutine(Cut(closestPoint, cuttingPlane.normal));
        
    }

    private IEnumerator Cut(Vector3 closestPoint, Vector3 normal)
    {
        yield return new WaitForSeconds(Random.Range(0, 0.12f));
        SlicedHull hull = gameObject.Slice(closestPoint, normal, mat);
        if (hull == null) yield break;
        yield return null;
        GameObject upperHull = hull.CreateUpperHull(gameObject);
        yield return null;
        GameObject lowerHull = hull.CreateLowerHull(gameObject);
        Transform hullParent = transform;
        if (upperHull != null)
        {
            yield return null;
            SetupHull(upperHull, hullParent);
            points++;
        }

        if (lowerHull != null)
        {
            yield return null;
            SetupHull(lowerHull, hullParent);
            points++;
        }
        yield return null;
        Deactivate();
    }
    public static uint points = 0;

    private void SetupHull(GameObject hull, Transform hullParent)
    {
        if (hull == null) return;
        BoxCollider boxCollider = hull.AddComponent<BoxCollider>();
        Rigidbody hullRb = hull.AddComponent<Rigidbody>();
        CuttableObject cuttableObject = hull.AddComponent<CuttableObject>();
        cuttableObject.mat = mat;
        hull.transform.position = hullParent.position;
        hull.transform.rotation = hullParent.rotation;
        hullRb.AddExplosionForce(
            100, hull.transform.position, 10);
        Destroy(hull, 0.5f + Random.Range(0f,1f));
        boxCollider.isTrigger = true;
    }
}