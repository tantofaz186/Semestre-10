using System.Collections.Generic;
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
            foreach (var module in baseModules)
            {
                var mod = Instantiate(module);
                mod.gameObject.SetActive(false);
                pooledModules.Add(mod);
            }
        }
        
        public Module GetModule()
        {
            foreach (var module in pooledModules)
            {
                if (!module.gameObject.activeInHierarchy)
                {
                    module.gameObject.SetActive(true);
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