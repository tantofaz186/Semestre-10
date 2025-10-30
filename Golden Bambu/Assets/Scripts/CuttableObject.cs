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
        CutManager.Instance.CutObject(this, closestPoint, cuttingPlane.normal);
    }

}