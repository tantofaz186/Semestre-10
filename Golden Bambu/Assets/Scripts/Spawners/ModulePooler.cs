using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Spawners
{
    public class ModulePooler : MonoBehaviour
    {
        [SerializeField] Module[] baseModules;

        public List<Module> pooledModules = new List<Module>();
        public static ModulePooler Instance;

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
            pooledModules.Sort((a,b) => Random.Range(-1,2));
            var module = pooledModules.First((m) => !m.gameObject.activeInHierarchy);
            module.gameObject.SetActive(true);
            return module;
        }

        public void ReturnModule(Module module)
        {
            module.gameObject.SetActive(false);
            module.transform.position = Vector3.zero;
            module.transform.rotation = Quaternion.identity;
        }
    }
}