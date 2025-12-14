using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class TitleDropAnimationController : MonoBehaviour
{
    public Animation anim;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(3);
        anim.Play();

    }

}
