using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class Cannon : MonoBehaviour
{
    public List<CuttableObject> objectsToSpawn;
    int next = 0;
    public List<Transform> shootPoint;
    private List<CuttableObject> spawnedObjects = new List<CuttableObject>();

    void Awake()
    {
        for (int i = 0; i < objectsToSpawn.Count; i++)
        {
            CuttableObject obj = Instantiate(objectsToSpawn[i]);
            spawnedObjects.Add(obj);
            spawnedObjects[i].gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        SpawnNextMusic();
    }

    public Vector3 force;

    public void SpawnNext()
    {
        var nextObject = spawnedObjects[next];
        nextObject.transform.position = shootPoint[Random.Range(0, shootPoint.Count)].position;
        nextObject.Reset();


        nextObject.rb.Sleep();
        nextObject.rb.AddForce(force, ForceMode.VelocityChange);


        next++;
        next = next % spawnedObjects.Count;
    }

    bool isSpawning = false;

    public void StopSpawning()
    {
        CancelInvoke();
        isSpawning = false;
    }
    public void SpawnNextPerSecond()
    {
        if (isSpawning) return;
        isSpawning = true;
        InvokeRepeating(nameof(SpawnNext), 0, 1f);

    }

    public void SpawnNextMusic()
    {
        if (isSpawning) return;
        isSpawning = true;
        
        InvokeRepeating(nameof(SpawnNext), 0, 1f);
        InvokeRepeating(nameof(SpawnNext), 1.25f, 2f);
    }
    [CustomEditor(typeof(Cannon))]
    public class Cannon_Inspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Spawn"))
            {
                var t = target as Cannon;
                t.SpawnNext();
            }

            if (GUILayout.Button("Spawn With time"))
            {
                var t = target as Cannon;
                t.SpawnNextPerSecond();
            }
        }
    }
}