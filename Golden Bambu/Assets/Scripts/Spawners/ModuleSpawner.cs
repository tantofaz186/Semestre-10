using System;

namespace Spawners
{
    using System.Collections.Generic;
    using UnityEngine;

    #if UNITY_EDITOR
    using UnityEditor;
    #endif

    public class ModuleSpawner : MonoBehaviour
    {
        private Module lastModule;
        private Queue<Module> spawnedModules = new Queue<Module>();
        private byte startModulesToSpawn = 4;
        private void Start()
        {
            spawnedModules.Enqueue(SpawnFirstModule());
            for (int i = 0; i < startModulesToSpawn; i++)
            {
                spawnedModules.Enqueue(SpawnModule());
            }
            Spawnpoint.OnPlayerPassed += SpawnpointEvent;
        }

        private void OnDestroy()
        {
            Spawnpoint.OnPlayerPassed -= SpawnpointEvent;
        }

        //since there are two spawn points passed per module, we return a module every two events
        private bool spawnToggle = true;
        private void SpawnpointEvent()
        {
            if (spawnToggle)
            {
                spawnedModules.Enqueue(SpawnModule());
                HandleDespawn();
            }
            spawnToggle = !spawnToggle;
        }
        
        // ensure that there is one module behind the player
        private byte ignoreFirstTwoEventsForDespawning = 2;
        private void HandleDespawn()
        {
            if (ignoreFirstTwoEventsForDespawning > 0)
            {
                ignoreFirstTwoEventsForDespawning--;
            }
            else
            {
                ModulePooler.Instance.ReturnModule(spawnedModules.Dequeue());
            }
        }

        public Module SpawnFirstModule()
        {
            Module module = ModulePooler.Instance.GetModule();
            Transform spawnPoint = transform;
            AlignModule(module, spawnPoint);
            lastModule = module;
            return module;
        }

        public Module SpawnModule()
        {
            Module module = ModulePooler.Instance.GetModule();
            Transform spawnPoint = PickRandomAvailableSpawnPoint(lastModule);
            AlignModule(module, spawnPoint);
            lastModule = module;
            return module;
        }

        private void AlignModule(Module module, Transform spawnPoint)
        {
            Transform randomSpawnPoint = PickRandomSpawnPoint(module);
            Transform moduleTransform = module.transform;
            moduleTransform.forward = -spawnPoint.forward;
            moduleTransform.position = spawnPoint.position - randomSpawnPoint.position;
            moduleTransform.RotateAround(randomSpawnPoint.position, randomSpawnPoint.up,
                -randomSpawnPoint.localEulerAngles.y);
        }

        private int lastUsedSpawnPoint;

        private Transform PickRandomSpawnPoint(Module module)
        {
            int random = Random.Range(0, module.spawnPoints.Length);
            lastUsedSpawnPoint = random;
            return module.spawnPoints[random];
        }

        private Transform PickRandomAvailableSpawnPoint(Module module)
        {
            int random = Random.Range(0, module.spawnPoints.Length);
            if (random == lastUsedSpawnPoint) random = (random + 1) % module.spawnPoints.Length;
            lastUsedSpawnPoint = random;
            return module.spawnPoints[random];
        }
        
    }
    #if UNITY_EDITOR
    [CustomEditor(typeof(ModuleSpawner))]
    public class ModuleSpawnerEditor : Editor
    {
        private ModuleSpawner _target;

        private void Awake()
        {
            _target = (ModuleSpawner)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Spawn First Module"))
            {
                _target.SpawnFirstModule();
            }

            if (GUILayout.Button("Spawn Module"))
            {
                _target.SpawnModule();
            }
        }
    }

    #endif
}