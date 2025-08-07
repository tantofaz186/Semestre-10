using UnityEngine;
using UnityEngine.InputSystem;

public class Sword : MonoBehaviour
{
    private InputAction touchAction, moveAction;
    private SwordControl control;
    Camera mainCamera;

    private void Awake()
    {
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

    private void OnTouchActionCanceled(InputAction.CallbackContext obj)
    {
        Debug.Log($"Position action canceled at {moveAction.ReadValue<Vector2>()}");
    }

    private void OnTouchActionStarted(InputAction.CallbackContext obj)
    {
        Debug.Log($"Position action Started at {moveAction.ReadValue<Vector2>()}");
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