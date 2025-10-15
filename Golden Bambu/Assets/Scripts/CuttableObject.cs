using System;
using EzySlice;
using UnityEngine;
using Plane = UnityEngine.Plane;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class CuttableObject : MonoBehaviour
{
    public Rigidbody rb { get; private set; }
    private Material mat;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mat = GetComponent<MeshRenderer>().sharedMaterial;
    }

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
        gameObject.SetActive(false);
        Sword.Instance.OnCut -= OnCut;
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

    private static void SetupHull(GameObject hull, Transform hullParent)
    {
        if (hull == null) return;
        var boxCollider = hull.AddComponent<BoxCollider>();
        Rigidbody hullRb = hull.AddComponent<Rigidbody>();
        hull.AddComponent<CuttableObject>();
        hull.transform.position = hullParent.position;
        hull.transform.rotation = hullParent.rotation;
        hullRb.AddExplosionForce(
            100, hull.transform.position, 10);
        Destroy(hull, 0.5f + Random.Range(0f,1f));
        boxCollider.isTrigger = true;
    }
}