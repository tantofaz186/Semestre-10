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
        touchAction.performed += OnTouchActionChanged;
        touchAction.started += OnTouchActionStarted;
        touchAction.canceled += OnTouchActionCanceled;
        moveAction = control.Player.Move;
        moveAction.Enable();
        
    }
    Vector3 swipeStartPosition;
    private void OnTouchActionCanceled(InputAction.CallbackContext obj)
    {
        Debug.Log($"Position action canceled at {moveAction.ReadValue<Vector2>()}");
        Vector3 swipeEndPosition = moveAction.ReadValue<Vector2>();
        swipeEndPosition.z = -1 * mainCamera.transform.position.z;
        swipeEndPosition = mainCamera.ScreenToWorldPoint(swipeEndPosition);

        OnSwipeEnd?.Invoke(swipeStartPosition, swipeEndPosition);
    }

    private void OnTouchActionStarted(InputAction.CallbackContext obj)
    {
        Debug.Log($"Position action Started at {moveAction.ReadValue<Vector2>()}");
        swipeStartPosition = moveAction.ReadValue<Vector2>();
        swipeStartPosition.z = -1 * mainCamera.transform.position.z;
        swipeStartPosition = mainCamera.ScreenToWorldPoint(swipeStartPosition);
    }

    private void OnTouchActionChanged(InputAction.CallbackContext obj)
    {
        // Vector3 pos = moveAction.ReadValue<Vector2>();
        // pos.z = -1 * mainCamera.transform.position.z;
        // transform.position = mainCamera.ScreenToWorldPoint(pos);
    }

    private void OnDisable()
    {
        touchAction.performed -= OnTouchActionChanged;
        touchAction.started -= OnTouchActionStarted;
        touchAction.canceled -= OnTouchActionCanceled;
        touchAction.Disable();
        control.Disable();
    }
}