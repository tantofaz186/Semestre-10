
namespace Packages.com.tantofaz186.tool.Editor
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(Module))]
    public class UmaTool : Editor
    {
        private static Module[] allModules;
        private static int moduleSelector = 0;
        private Module module;

        protected void OnEnable()
        {
            allModules = Resources.LoadAll<Module>("Modules");
            module = target as Module;
        }

        bool _instantiated = false;
        private Module spawned;
        private Transform spawnPointSpawned;
        private float angleSpawned;

        protected virtual void OnSceneGUI()
        {
            if (!_instantiated)
            {
                for (int i = 0; i < module.spawnPoints.Length; i++)
                {
                    if (module.occupied[i]) continue;
                    Transform spawnPoint = module.spawnPoints[i];
                    if (Handles.Button(spawnPoint.position, spawnPoint.rotation, 2 * module.transform.localScale.magnitude / 3,
                            2 * module.transform.localScale.magnitude / 3, Handles.RectangleHandleCap))
                    {
                        spawned = InstanciateModule(allModules[moduleSelector], spawnPoint);
                        module.occupied[i] = true;
                        angleSpawned = spawnPoint.rotation.eulerAngles.y;
                        spawnPointSpawned = spawnPoint;
                        _instantiated = true;
                    }
                }
            }
            else
            {
                for (int i = -1; i <= 1; i += 2)
                {
                    if (Handles.Button(
                            spawnPointSpawned.position +
                            (spawnPointSpawned.transform.right * (i * (2 * module.transform.localScale.magnitude / 3))),
                            Quaternion.Euler(0, angleSpawned + (90 * i), 0),
                            2 * module.transform.localScale.magnitude / 3,
                            2 * module.transform.localScale.magnitude / 3,
                            Handles.ConeHandleCap))
                    {
                        moduleSelector = (moduleSelector + i + allModules.Length) % allModules.Length;
                        var aux = InstanciateModule(allModules[moduleSelector], spawnPointSpawned);
                        DestroyImmediate(spawned.gameObject);
                        spawned = aux;
                    }
                }

                if (Handles.Button(module.transform.position,
                        Quaternion.Euler(0, angleSpawned, 0),
                        2 * module.transform.localScale.magnitude / 6,
                        2 * module.transform.localScale.magnitude / 6,
                        Handles.SphereHandleCap))
                {
                    _instantiated = false;
                }

                if (Handles.Button(spawned.transform.position,
                        Quaternion.Euler(0, angleSpawned, 0),
                        2 * spawned.transform.localScale.magnitude / 6,
                        2 * spawned.transform.localScale.magnitude / 6,
                        Handles.SphereHandleCap))
                {
                    Selection.activeGameObject = spawned.gameObject;
                }
            }
        }

        private Module InstanciateModule(Module _module, Transform spawnPoint)
        {
            Module instantiated = Instantiate(_module, _module.transform.position, Quaternion.identity);
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