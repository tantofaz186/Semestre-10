namespace Packages.com.tantofaz186.tool.Editor
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(SampleModule))]
    public class ModuleSpawnerTool : Editor
    {
        private static SampleModule[] allModules;
        private static int moduleSelector = 0;
        private SampleModule module;
        private bool isPickingModule = false;
        private SampleModule instanciatedModule;
        private Transform spawnPointSpawned;
        private float angleSpawned;
        protected void OnEnable()
        {
            allModules = Resources.LoadAll<SampleModule>("Modules");
            module = target as SampleModule;
        }

        protected virtual void OnSceneGUI()
        {
            if (!isPickingModule)
            {
                for (int i = 0; i < module.spawnPoints.Length; i++)
                {
                    if (module.occupied[i]) continue;
                    Transform spawnPoint = module.spawnPoints[i];
                    if (SpawnNewModuleButton(spawnPoint))
                    {
                        instanciatedModule = InstanciateModule(allModules[moduleSelector], spawnPoint);
                        module.occupied[i] = true;
                        angleSpawned = spawnPoint.rotation.eulerAngles.y;
                        spawnPointSpawned = spawnPoint;
                        isPickingModule = true;
                    }
                }
            }
            else
            {
                for (int i = -1; i <= 1; i += 2)
                {
                    if (NextOrPreviousButton(i))
                    {
                        PickNextOrPreviousModule(i);
                    }
                }

                if (SelectSelfButton())
                {
                    isPickingModule = false;
                }

                if (SelectInstanciatedModuleButton())
                {
                    Selection.activeGameObject = instanciatedModule.gameObject;
                }
            }
        }

        private bool SpawnNewModuleButton(Transform spawnPoint)
        {
            Handles.color = Color.blue;
            return Handles.Button(spawnPoint.position, spawnPoint.rotation, 2 * module.transform.localScale.magnitude / 3,
                2 * module.transform.localScale.magnitude / 3, Handles.RectangleHandleCap);
        }

        private bool SelectInstanciatedModuleButton() => SelectModuleButton(instanciatedModule.transform);

        private bool SelectSelfButton() => SelectModuleButton(module.transform);

        private bool SelectModuleButton(Transform moduleTransform)
        {
            Handles.color = Color.green;

            return Handles.Button(moduleTransform.position,
                Quaternion.identity,
                moduleTransform.localScale.magnitude / 3,
                 moduleTransform.localScale.magnitude / 3,
                Handles.SphereHandleCap);
        }

        private bool NextOrPreviousButton(int i)
        {
            Handles.color = Color.blue;
            return Handles.Button(
                spawnPointSpawned.position +
                (spawnPointSpawned.transform.right * (i * (2 * module.transform.localScale.magnitude / 3))),
                Quaternion.Euler(0, angleSpawned + (90 * i), 0),
                2 * module.transform.localScale.magnitude / 3,
                2 * module.transform.localScale.magnitude / 3,
                Handles.ConeHandleCap);
        }

        private void PickNextOrPreviousModule(int i)
        {
            moduleSelector = (moduleSelector + i + allModules.Length) % allModules.Length;
            var aux = InstanciateModule(allModules[moduleSelector], spawnPointSpawned);
            DestroyImmediate(instanciatedModule.gameObject);
            instanciatedModule = aux;
        }

        private SampleModule InstanciateModule(SampleModule _module, Transform spawnPoint)
        {
            SampleModule instantiated = Instantiate(_module, _module.transform.position, Quaternion.identity);
            int next = Mathf.RoundToInt(Random.value * instantiated.spawnPoints.Length) % instantiated.spawnPoints.Length;

            instantiated.transform.forward = -spawnPoint.forward;
            instantiated.transform.position = _module.transform.position -
                                              (instantiated.spawnPoints[next].position - spawnPoint.position);

            instantiated.transform.RotateAround(instantiated.spawnPoints[next].position, instantiated.spawnPoints[next].up,
                -instantiated.spawnPoints[next].localEulerAngles.y);
            instantiated.occupied[next] = true;
            return instantiated;
        }
    }
}