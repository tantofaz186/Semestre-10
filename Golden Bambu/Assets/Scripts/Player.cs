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
        Sword.Instance.OnSwipeEnd += MoveLane;
    }

    private void MoveLane(Vector3 start, Vector3 end)
    {
        // Vector3 direction = (end - start).normalized;
        // Vector3 right = transform.right;
        // float dot = Vector3.Dot(direction, right);
        // if (Mathf.Abs(dot) > 0.5f)
        // {
        //     Vector3 moveDirection = dot > 0 ? right : -right;
        //     rb.AddForce(moveDirection * 10, ForceMode.VelocityChange);
        // }
    }

    public void OnDestroy()
    {
        Sword.Instance.OnCut -= TriggerCutAnimation;
        Sword.Instance.OnSwipeEnd -= MoveLane;
    }

    private void TriggerCutAnimation(Plane plane)
    {
        anim.SetTrigger(Attack);
    }
}
