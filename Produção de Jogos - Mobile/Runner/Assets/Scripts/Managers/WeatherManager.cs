using System.Collections;
using Collectables;
using UnityEngine;

namespace Managers
{
    public class WeatherManager : MonoBehaviour
    {
        Light sceneLight;
        Color originalLightColor;
        private void Awake()
        {
            sceneLight = GetComponent<Light>();
            originalLightColor = sceneLight.color;
            
        }

        private void Start()
        {
            Cloud.OnCloudCollected += HandleCloudCollected;
        }
        private void OnDestroy()
        {
            Cloud.OnCloudCollected -= HandleCloudCollected;
        }
        
        private void HandleCloudCollected()
        {
            StartCoroutine(ChangeColorsRandomly());
        }

        private IEnumerator ChangeColorsRandomly()
        {
            float duration = 5f;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                sceneLight.color = new Color(Random.value, Random.value, Random.value);
                elapsed += 0.5f;
                yield return new WaitForSeconds(0.5f);
            }
            sceneLight.color = originalLightColor;
        }
    }
}
