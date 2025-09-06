using UnityEngine;

public class Chave : MonoBehaviour
{
    public Material material;
    public bool picked;
    private void Awake()
    {
        foreach (var mr in GetComponentsInChildren<MeshRenderer>())
        {
            mr.material = material;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (picked) return;
        if (other.CompareTag("Player"))
        {
            picked = true;
            foreach (var mr in GetComponentsInChildren<MeshRenderer>())
            {
                mr.enabled = false;
            }
        }
    }
}
