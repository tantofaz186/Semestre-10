using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Spawners
{
    public class ModuleSpawner : MonoBehaviour
    {
        private Module lastModule;

        private void Start()
        {
            SpawnFirstModule();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpawnModule();
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
            Transform spawnPoint = lastModule.spawnPoints[1];
            AlignModule(module, spawnPoint);
            lastModule = module;
            return module;
        }
        private List<Module> markedForDeletion = new List<Module>();
        private int i = 0; 
        public void SpawnUntilTurn()
        {

            if (i++%2 == 0)
            {
                foreach (Module module in markedForDeletion)
                {
                    ModulePooler.Instance.ReturnModule(module);
                }
            }
            if (lastModule == null)
            {
                SpawnFirstModule();
            }
            markedForDeletion.Add(lastModule);
            int failsafe = 0;
            do
            {
                markedForDeletion.Add(SpawnModule());
                failsafe++;
            } while (!(lastModule.gameObject.name.Contains("Turn") || lastModule.gameObject.name.Contains("Fork") || failsafe > 10));
            markedForDeletion.Remove(lastModule);
        }

        private void AlignModule(Module module, Transform spawnPoint)
        {
            Transform moduleTransform = module.transform;
            moduleTransform.forward = -spawnPoint.forward;
            moduleTransform.position = spawnPoint.position - module.spawnPoints[0].position;
            moduleTransform.RotateAround(module.spawnPoints[0].position, module.spawnPoints[0].up,
                -module.spawnPoints[0].localEulerAngles.y);
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

            if (GUILayout.Button("Spawn Until Turn"))
            {
                _target.SpawnUntilTurn();
            }
        }
    }

    #endif
}