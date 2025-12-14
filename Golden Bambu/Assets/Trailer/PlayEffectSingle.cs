using UnityEngine;
using System.Collections;
using UnityEngine.VFX;

public class PlayEffectSingle : MonoBehaviour
{
    
    public VisualEffect effect;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return new WaitForSeconds(3);
        effect.Play();

    }
}
