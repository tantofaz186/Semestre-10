using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using TouchPhase = UnityEngine.TouchPhase;

public class Sword : MonoBehaviour
{
    private static Sword instance;
    public static Sword Instance => instance;

    [SerializeField] private const float deadzone = 4.25f;
    private InputAction touchAction, moveAction;
    private SwordControl control;
    Camera mainCamera;
    [SerializeField] VisualEffect swordTrailVFX;
    [SerializeField] private Player player;
    public delegate void Swipe(Vector3 start, Vector3 end);

    public delegate void SwipePlane(Plane plane);

    public event Swipe OnSwipeEnd;
    public event SwipePlane OnCut;
    Plane cuttingPlane;

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
    }

    Vector3 swipeStartPosition;

    private void OnTouchActionCanceled(InputAction.CallbackContext obj)
    {
        Vector3 swipeEndPosition = TreatPosition(moveAction.ReadValue<Vector2>());
        float distance = Vector3.Distance(swipeStartPosition, swipeEndPosition);
        if (deadzone < distance)
        {
            Vector3 mainCameraPosition = mainCamera.transform.position;

            OnSwipeEnd?.Invoke(swipeStartPosition, swipeEndPosition);
            cuttingPlane.Set3Points(
                swipeStartPosition,
                swipeEndPosition,
                mainCameraPosition);
            SetupVfx(swipeEndPosition, mainCameraPosition, cuttingPlane.normal);
            OnCut?.Invoke(cuttingPlane);
        }
    }

    private void SetupVfx(Vector3 swipeEndPosition, Vector3 mainCameraPosition, Vector3 planeNormal)
    {
        Vector3 midPoint = (swipeEndPosition + swipeStartPosition) / 2;
        swordTrailVFX.transform.position = new(midPoint.x, player.transform.position.y, player.transform.position.z);
        swordTrailVFX.transform.LookAt(midPoint, planeNormal);
        swordTrailVFX.Play();
    }

    private void OnTouchActionStarted(InputAction.CallbackContext obj)
    {
        swipeStartPosition = TreatPosition(moveAction.ReadValue<Vector2>());
    }

    private Vector3 TreatPosition(Vector2 untreatedPosition)
    {
        Vector3 treatedPosition = untreatedPosition;
        treatedPosition.z = /*player.transform.position.z + */11;
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