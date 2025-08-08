using UnityEngine;
using UnityEngine.InputSystem;

public class Sword : MonoBehaviour
{
    private static Sword instance;
    public static Sword Instance => instance;

    private InputAction touchAction, moveAction;
    private SwordControl control;
    Camera mainCamera;
    public delegate void Swipe(Vector3 start, Vector3 end);
    public event Swipe OnSwipeEnd;
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