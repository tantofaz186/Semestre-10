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

        private void Start()
        {
            SpawnFirstModule();
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

        private List<Module> markedForDeletion = new List<Module>();
        private int i = 0;

        public void SpawnUntilTurn()
        {
            if (i++ % 2 == 0)
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