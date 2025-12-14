using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class TitleDropAnimationController : MonoBehaviour
{
    public Animation anim;
    public VisualEffect effect;

    private void Awake()
    {
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return new WaitForSeconds(3);
        effect.Play();
        yield return new WaitForSeconds(0.25f);
        anim.Play();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
