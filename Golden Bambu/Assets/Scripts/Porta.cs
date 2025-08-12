using System;
using System.Collections;
using UnityEngine;

public class Porta : MonoBehaviour
{
    [SerializeField]
    private Chave chave;

    private void Awake()
    {
        GetComponent<MeshRenderer>().material = chave.material;
    }


    bool opening = false;
    IEnumerator Open()
    {
        Debug.Log("Opening");
        opening = true;
        float time = 0;
        while (time < 2)
        {
            float elapsed = Time.deltaTime;
            transform.position += Vector3.down * (2f * elapsed);
            time += elapsed;
            Debug.Log(time);
            yield return new WaitForEndOfFrame();
        }
        opening = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("CollisionEnter");
        if(!opening && chave.picked && other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Open());
        }
    }
}
