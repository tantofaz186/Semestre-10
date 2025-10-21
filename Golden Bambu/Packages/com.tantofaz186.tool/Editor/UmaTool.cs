using UnityEngine;

namespace Packages.com.tantofaz186.tool.Editor
{
    using UnityEditor;

    [CustomEditor(typeof(Module))]
    public class UmaTool : Editor
    {
        protected virtual void OnSceneGUI()
        {
            Module module = target as Module;
            foreach (var spawnPoint in module.spawnPoints)
            {
                if (Handles.Button(spawnPoint.transform.position, spawnPoint.transform.rotation, 10, 20, Handles.RectangleHandleCap))
                {
                    Debug.Log(spawnPoint.transform.position);
                    Debug.Log(spawnPoint.transform.rotation);
                    Module instantiated = Instantiate(module, spawnPoint.transform.position, Quaternion.identity);
                    instantiated.transform.forward = -spawnPoint.forward;
                    instantiated.transform.position = spawnPoint.position - instantiated.spawnPoints[0].position;
                    instantiated.transform.RotateAround(spawnPoint.transform.position, spawnPoint.transform.up,
                        -spawnPoint.transform.localEulerAngles.y);
                };
            }

        }
    }
}