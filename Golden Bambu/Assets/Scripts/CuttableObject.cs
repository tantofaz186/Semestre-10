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
        SlicedHull hull = gameObject.Slice(cuttingPlane.ClosestPointOnPlane(transform.position), cuttingPlane.normal, mat);
        if (hull == null) return;
        GameObject upperHull = hull.CreateUpperHull(gameObject);
        GameObject lowerHull = hull.CreateLowerHull(gameObject);
        Transform hullParent = transform;
        if (upperHull != null)
        {
            SetupHull(upperHull, hullParent);
            points++;
        }

        if (lowerHull != null)
        {
            SetupHull(lowerHull, hullParent);
            points++;
        }

        Debug.Log(points);
        Deactivate();
    }

    private static uint points = 0;

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