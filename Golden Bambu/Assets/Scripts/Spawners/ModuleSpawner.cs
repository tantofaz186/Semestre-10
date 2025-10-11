using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Spawners
{
    public class ModuleSpawner : MonoBehaviour
    {
        private Module lastModule;
        
        public void SpawnFirstModule()
        {
            Module module = ModulePooler.Instance.GetModule();
            Transform spawnPoint = transform;
            Transform moduleTransform = module.transform;
            moduleTransform.position = spawnPoint.position;
            moduleTransform.rotation = spawnPoint.rotation;
            lastModule = module;
        }
        
        public void SpawnModule()
        {
            Module module = ModulePooler.Instance.GetModule();
            Transform spawnPoint = lastModule.spawnPoints[1];
            Transform moduleTransform = module.transform;
            moduleTransform.position = spawnPoint.position;
            moduleTransform.rotation = spawnPoint.rotation;
            lastModule = module;
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