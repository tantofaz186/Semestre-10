using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Cannon : MonoBehaviour
{
    public List<CuttableObject> objectsToSpawn;
    int next = 0;
    public List<Transform> shootPoint;
    private List<CuttableObject> spawnedObjects = new List<CuttableObject>();

    private void Start()
    {
        for(int j = 0; j < 2; j++)
            for (int i = 0; i < objectsToSpawn.Count; i++)
            {
                CuttableObject obj = Instantiate(objectsToSpawn[i]);
                spawnedObjects.Add(obj);
                spawnedObjects[i].Deactivate();
                objectsToSpawn[i].Deactivate();
            }
            
            AudioController.OnMusicStart += SpawnNextMusic;
    }

    private void OnDestroy()
    {
        AudioController.OnMusicStart -= SpawnNextMusic;
    }

    public Vector3 force;
    public float torque;
    public void SpawnNext()
    {
        var nextObject = spawnedObjects[next];
        nextObject.Reset();
        nextObject.transform.position = shootPoint[Random.Range(0, shootPoint.Count)].position;


        nextObject.rb.Sleep();
        nextObject.rb.AddTorque(nextObject.transform.right * torque, ForceMode.VelocityChange);
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

    public void SpawnNextMusic(AudioClipWithTempo clipWithTempo)
    {
        StopSpawning();
        isSpawning = true;
        foreach (var spawningTempo in clipWithTempo.spawningTempos)
        {
            InvokeRepeating(nameof(SpawnNext), spawningTempo * clipWithTempo.musicTempo, clipWithTempo.musicLoopTime);
        }
    }
    #if UNITY_EDITOR
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
    #endif
}
