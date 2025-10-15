using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Player : MonoBehaviour
{
    public float speed;
    private Rigidbody rb;
    private Collider col;
    private const float MAX_SPEED = 20;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude < MAX_SPEED)
        {
            rb.AddForce(transform.forward * speed, ForceMode.Acceleration);
        }
    }
}
