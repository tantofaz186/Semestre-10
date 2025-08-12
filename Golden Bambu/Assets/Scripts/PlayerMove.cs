using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;

    private void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized * (speed * Time.deltaTime);
        transform.Translate(movement);
    }
}
