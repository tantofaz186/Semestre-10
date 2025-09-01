using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class Cannon : MonoBehaviour
{
    public List<CuttableObject> objectsToSpawn;
    int next = 0;
    public Transform shootPoint;
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

    public Vector3 force;

    public void SpawnNext()
    {
        var nextObject = spawnedObjects[next];
        nextObject.transform.position = shootPoint.position;
        nextObject.rb.Sleep();
        nextObject.Reset();
        nextObject.rb.AddForce(force, ForceMode.Impulse);


        next++;
        next = next % spawnedObjects.Count;
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
        }
    }
}