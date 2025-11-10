using System.Collections;
using EzySlice;
using Unity.VisualScripting;
using UnityEngine;

public class CutManager : MonoBehaviour
{
    public static CutManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
    }

    public void CutObject(CuttableObject objToCut, Vector3 closestPoint, Vector3 normal)
    {
        StartCoroutine(Cut(objToCut, closestPoint, normal));
    }

    public void CutAllObjects()
    {
        CuttableObject[] objs = FindObjectsByType<CuttableObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (CuttableObject obj in objs)
        {
            Transform objTransform = obj.transform;
            Vector3 closestPoint = objTransform.position;
            Vector3 normal = Vector3.up;
            SimpleCut(obj, closestPoint, normal);            
            normal = Vector3.right;
            SimpleCut(obj, closestPoint, normal);
            normal = Vector3.one;
            SimpleCut(obj, closestPoint, normal);
            normal = Vector3.zero;
            SimpleCut(obj, closestPoint, normal);
            // StartCoroutine(Cut(obj, closestPoint, normal));
        }
    }

    private void SimpleCut(CuttableObject objToCut, Vector3 closestPoint, Vector3 normal)
    {
        var t = objToCut.transform;
        Vector3 position = t.position;
        Quaternion rotation = t.rotation;
        Material mat = objToCut.mat;
        GameObject o;
        SlicedHull hull = (o = objToCut.gameObject).Slice(closestPoint, normal, mat);
        if(hull == null) return;
        GameObject upperHull = hull.CreateUpperHull(o);
        GameObject lowerHull = hull.CreateLowerHull(objToCut.gameObject);
        objToCut.Deactivate();
        SetupHull(upperHull, position, rotation, mat);
        SetupHull(lowerHull, position, rotation, mat);
    }

    private IEnumerator Cut(CuttableObject objToCut, Vector3 closestPoint, Vector3 normal)
    {
        yield return new WaitForSeconds(Random.Range(0, 0.12f));
        if (objToCut.IsUnityNull()) yield break;
        Vector3 position = objToCut.transform.position;
        Quaternion rotation = objToCut.transform.rotation;
        Material mat = objToCut.mat;
        SlicedHull hull = objToCut.gameObject.Slice(closestPoint, normal, mat);
        if (hull == null) yield break;
        GameObject upperHull = hull.CreateUpperHull(objToCut.gameObject);
        GameObject lowerHull = hull.CreateLowerHull(objToCut.gameObject);
        objToCut.Deactivate();
        if (!upperHull.IsUnityNull())
        {
            yield return null;
            SetupHull(upperHull, position, rotation, mat);
            points++;
        }

        if (!lowerHull.IsUnityNull())
        {
            yield return null;
            SetupHull(lowerHull, position, rotation, mat);
            points++;
        }

        yield return null;
    }

    private void SetupHull(GameObject hull, Vector3 position, Quaternion rotation, Material mat)
    {
        if (hull.IsUnityNull()) return;
        BoxCollider boxCollider = hull.AddComponent<BoxCollider>();
        Rigidbody hullRb = hull.AddComponent<Rigidbody>();
        CuttableObject cuttableObject = hull.AddComponent<CuttableObject>();
        cuttableObject.mat = mat;
        cuttableObject.col = boxCollider;
        hull.transform.position = position;
        hull.transform.rotation = rotation;
        hullRb.Sleep();
        hullRb.AddExplosionForce(
            100, position, 10);
        Destroy(hull, 0.5f + Random.Range(0f, 1f));
        boxCollider.isTrigger = true;
    }

    public uint points = 0;
}