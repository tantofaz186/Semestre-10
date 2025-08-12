using System;
using System.Collections;
using UnityEngine;

public class MoveUpAndDown : MonoBehaviour
{
    private Vector3 startPos;

    private void Awake()
    {
        startPos = transform.position;
        StartCoroutine(MoveUpAndDownRoutine());
    }

    private IEnumerator MoveUpAndDownRoutine()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(0.5f);
            transform.position = startPos;
            yield return new WaitForSeconds(0.5f);
            transform.position = startPos +  Vector3.up;
        }
    }
}
