using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Player : MonoBehaviour
{
    private static readonly int Attack = Animator.StringToHash("Attack");
    public float speed;
    private Rigidbody rb;
    private Collider col;
    private Animator anim;
    private const float MAX_SPEED = 16;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        anim = GetComponent<Animator>();
    }

    public void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude < MAX_SPEED)
        {
            rb.AddForce(transform.forward * speed, ForceMode.Acceleration);
        }
    }

    public void Start()
    {
        Sword.Instance.OnCut += TriggerCutAnimation;
    }

    public void OnDestroy()
    {
        Sword.Instance.OnCut -= TriggerCutAnimation;
    }

    private void TriggerCutAnimation(Plane plane)
    {
        anim.SetTrigger(Attack);
    }
}
