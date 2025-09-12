using UnityEngine;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.TouchPhase;

public class Sword : MonoBehaviour
{
    private static Sword instance;
    public static Sword Instance => instance;

    [SerializeField]
    private float deadzone = 0.25f;
    private InputAction touchAction, moveAction;
    private SwordControl control;
    Camera mainCamera;
    public LineRenderer lineRenderer;

    public delegate void Swipe(Vector3 start, Vector3 end);

    public event Swipe OnSwipeEnd;
    private readonly Plane plane = new Plane(-Vector3.forward, Vector3.zero);

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        mainCamera = Camera.main;
        control = new SwordControl();
        control.Enable();
        touchAction = control.Player.Touch;
        touchAction.Enable();
        moveAction = control.Player.Move;
        moveAction.Enable();
        touchAction.started += OnTouchActionStarted;
        touchAction.canceled += OnTouchActionCanceled;
        lineRenderer = GetComponent<LineRenderer>();
    }

    Vector3 swipeStartPosition;

    private void OnTouchActionCanceled(InputAction.CallbackContext obj)
    {
        Vector3 swipeEndPosition = TreatPosition(moveAction.ReadValue<Vector2>());
        if(deadzone < Vector3.Distance(transform.position, swipeEndPosition))
            OnSwipeEnd?.Invoke(swipeStartPosition, swipeEndPosition);
    }

    private void OnTouchActionStarted(InputAction.CallbackContext obj)
    {
        swipeStartPosition = TreatPosition(moveAction.ReadValue<Vector2>());
    }

    public void SetLine(Ray ray, float magnitude)
    {
        lineRenderer.SetPositions(new[]
        {
            ray.origin, ray.origin + ray.direction * magnitude
        });
    }

    public void SetLine(Vector3 a, Vector3 b)
    {
        lineRenderer.SetPositions(new[]
        {
            a, b
        });
    }

    public Vector3 GetCameraPosition()
    {
        return mainCamera.transform.position;
    }
    private Vector3 TreatPosition(Vector2 untreatedPosition)
    {
        Vector3 treatedPosition = untreatedPosition;
        treatedPosition.z = -1 * mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(treatedPosition);
    }

    private void OnDisable()
    {
        touchAction.started -= OnTouchActionStarted;
        touchAction.canceled -= OnTouchActionCanceled;
        touchAction.Disable();
        control.Disable();
    }
}