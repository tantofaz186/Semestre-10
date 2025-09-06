using UnityEngine;
using UnityEngine.InputSystem;

public class Sword : MonoBehaviour
{
    private static Sword instance;
    public static Sword Instance => instance;

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
        touchAction.started += OnTouchActionStarted;
        touchAction.canceled += OnTouchActionCanceled;
        moveAction = control.Player.Move;
        moveAction.Enable();
        lineRenderer = GetComponent<LineRenderer>();
    }

    Vector3 swipeStartPosition;

    private void OnTouchActionCanceled(InputAction.CallbackContext obj)
    {
        Vector3 swipeEndPosition = TreatPosition(moveAction.ReadValue<Vector2>());
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

    private Vector3 TreatPosition(Vector2 untreatedPosition)
    {
        Vector3 treatedPosition = untreatedPosition;
        treatedPosition.z = -1 * mainCamera.transform.position.z;
        /*
            Ray ray = mainCamera.ScreenPointToRay(treatedPosition);
            if (plane.Raycast(ray, out var dist))
            {
                Vector3 worldPos = ray.GetPoint(dist);
                Debug.Log(worldPos);
                return worldPos;
            }
    
            return Vector3.zero;
         */
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