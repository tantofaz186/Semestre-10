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
                if (Handles.Button(spawnPoint.position, spawnPoint.rotation, 2 * module.transform.localScale.magnitude/3, 2 * module.transform.localScale.magnitude/3, Handles.RectangleHandleCap))
                {
                    Module instantiated = Instantiate(module, module.transform.position, Quaternion.identity);
                    int next = Mathf.RoundToInt(Random.value * instantiated.spawnPoints.Length) % instantiated.spawnPoints.Length;

                    instantiated.transform.forward = -spawnPoint.forward;
                    instantiated.transform.position = module.transform.position -
                                                      (instantiated.spawnPoints[next].position - spawnPoint.position);
                    
                    instantiated.transform.RotateAround(instantiated.spawnPoints[next].position, instantiated.spawnPoints[next].up,
                        -instantiated.spawnPoints[next].localEulerAngles.y);
                }
                
            }
        }
    }
}