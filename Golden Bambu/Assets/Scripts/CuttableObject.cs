using UnityEngine;
using Plane = UnityEngine.Plane;
using Random = UnityEngine.Random;

public class CuttableObject : MonoBehaviour
{
    public Rigidbody rb;
    public Material mat;
    public Collider col;

    private void Start()
    {
        Sword.Instance.OnCut += OnCut;
    }

    public void Reset()
    {
        gameObject.SetActive(true);
        Sword.Instance.OnCut += OnCut;
    }

    private readonly Vector3 resetPosition = Vector3.one * -10;

    public void Deactivate()
    {
        Sword.Instance.OnCut -= OnCut;
        transform.position = resetPosition;
        transform.rotation = Random.rotation;
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Sword.Instance.OnCut -= OnCut;
    }

    private void OnCut(Plane cuttingPlane)
    {
        Vector3 closestPoint = cuttingPlane.ClosestPointOnPlane(transform.position);
        if ((transform.position - closestPoint).sqrMagnitude > col.bounds.size.sqrMagnitude) return;
        CutManager.Instance.CutObject(this, closestPoint, cuttingPlane.normal);
    }
}