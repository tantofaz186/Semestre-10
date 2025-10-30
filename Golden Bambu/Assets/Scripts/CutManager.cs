using System.Collections;
using EzySlice;
using Unity.VisualScripting;
using UnityEngine;

public class CutManager : MonoBehaviour
{
    public static CutManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void CutObject(CuttableObject objToCut, Vector3 closestPoint, Vector3 normal)
    {
        StartCoroutine(Cut(objToCut, closestPoint, normal));
        
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
            SetupHull(upperHull, position,rotation,mat);
            points++;
        }

        if (!lowerHull.IsUnityNull())
        {
            yield return null;
            SetupHull(lowerHull, position,rotation,mat);
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
        hull.transform.position = position;
        hull.transform.rotation = rotation;
        hullRb.Sleep();
        hullRb.AddExplosionForce(
            100, position, 10);
        Destroy(hull, 0.5f + Random.Range(0f,1f));
        boxCollider.isTrigger = true;
    }
    public uint points = 0;
}
