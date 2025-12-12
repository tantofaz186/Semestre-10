using UnityEngine;
using System.Collections.Generic;
using UnityEngine.VFX;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Cannon : MonoBehaviour
{
    public List<CuttableObject> objectsToSpawn;
    int next = 0;
    public List<Transform> shootPoint;
    private List<CuttableObject> spawnedObjects = new List<CuttableObject>();
    [SerializeField] private VisualEffect portalEffect;
    [SerializeField] private List<VisualEffect> portalEffectList;

    private void Start()
    {
        for (int j = 0; j < 2; j++)
        for (int i = 0; i < objectsToSpawn.Count; i++)
        {
            CuttableObject obj = Instantiate(objectsToSpawn[i]);
            spawnedObjects.Add(obj);
            spawnedObjects[i].Deactivate();
            objectsToSpawn[i].Deactivate();
        }

        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            var portal = Instantiate(portalEffect, portalEffect.transform.position, portalEffect.transform.rotation);
            portalEffectList.Add(portal);
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
        var nextEffect = portalEffectList[next];
        var pointPosition = shootPoint[Random.Range(0, shootPoint.Count)].position;
        nextObject.transform.position = pointPosition;
        nextEffect.transform.position = pointPosition;
        nextEffect.Play();


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