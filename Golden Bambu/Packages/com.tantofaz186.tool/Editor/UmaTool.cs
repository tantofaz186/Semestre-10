using UnityEngine;

namespace Packages.com.tantofaz186.tool.Editor
{
    using UnityEditor;

    [CustomEditor(typeof(Module))]
    public class UmaTool : Editor
    {
        private Module module;

        protected void OnEnable()
        {
            module = target as Module;
        }

        protected virtual void OnSceneGUI()
        {
            foreach (Transform spawnPoint in module.spawnPoints)
            {
                if (Handles.Button(spawnPoint.position, spawnPoint.rotation, 2, 2, Handles.RectangleHandleCap))
                {
                    Module instantiated = Instantiate(module, module.transform.position, Quaternion.identity);
                    instantiated.transform.forward = -spawnPoint.forward;
                    instantiated.transform.position = module.transform.position -
                                                      (instantiated.spawnPoints[0].position - spawnPoint.position);

                    instantiated.transform.RotateAround(instantiated.spawnPoints[0].position, instantiated.spawnPoints[0].up,
                        -instantiated.spawnPoints[0].localEulerAngles.y);
                }
            }
        }
    }
}