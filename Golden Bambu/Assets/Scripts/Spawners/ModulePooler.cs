using System.Collections.Generic;
using UnityEngine;

namespace Spawners
{
    public class ModulePooler : MonoBehaviour
    {
        [SerializeField] Module[] baseModules;

        public List<Module> pooledModules = new List<Module>();
        public static ModulePooler Instance;
        Queue<Module> markForReuse = new Queue<Module>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            for (int i = 0; i < 3; i++)
            {
                foreach (var module in baseModules)
                {
                    var mod = Instantiate(module);
                    mod.gameObject.SetActive(false);
                    pooledModules.Add(mod);
                }
            }
        }

        public Module GetModule()
        {
            if (markForReuse.Count >= 3)
            {
                ReturnModule(markForReuse.Dequeue());
            }

            foreach (var module in pooledModules)
            {
                if (!module.gameObject.activeInHierarchy)
                {
                    module.gameObject.SetActive(true);
                    markForReuse.Enqueue(module);
                    return module;
                }
            }

            var newModule = Instantiate(baseModules[Random.Range(0, baseModules.Length)]);
            pooledModules.Add(newModule);
            return newModule;
        }

        public void ReturnModule(Module module)
        {
            module.gameObject.SetActive(false);
            module.transform.position = Vector3.zero;
            module.transform.rotation = Quaternion.identity;
        }
    }
}