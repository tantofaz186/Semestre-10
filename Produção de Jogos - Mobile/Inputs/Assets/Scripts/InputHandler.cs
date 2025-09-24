using System;
using System.Collections;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public event Action onSingleTap;
    public event Action onDoubleTap;
    public event Action onTripleTap;
    public event Action<Direction> onSwipe;
    public event Action<Vector3> onTilt;

    [SerializeField]
    private float tapDelayThreshold = 0.4f;

    [SerializeField]
    private float swipeMinimalDistance = 75f;

    private Vector2 touchBeginPosition;
    private float touchEndTime;
    private int tapCount = 0;
    private const int MAX_TAP_COUNT = 3;
    private bool handlingTaps = false;

    private static InputHandler instance;
    public static InputHandler Instance => instance;

    #region unity functions

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        HandleTouch();
        GetTilt();
    }

    private void HandleTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    HandleTouchBegin(touch);
                    break;
                case TouchPhase.Ended:
                    HandleTouchEnd(touch);
                    break;
            }
        }
    }

    #endregion

    #region touch

    private void HandleTouchBegin(Touch touch)
    {
        touchBeginPosition = touch.position;
    }

    private void HandleTouchEnd(Touch touch)
    {
        if (Vector3.Distance(touchBeginPosition, touch.position) > swipeMinimalDistance)
            HandleSwipe(touchBeginPosition, touch.position);
        else
        {
            touchEndTime = Time.time;
            StartCoroutine(HandleTaps());
        }
    }

    private IEnumerator HandleTaps()
    {
        tapCount++;
        if (handlingTaps) yield break;
        handlingTaps = true;
        //Sim, Diegão, se fizer assim, o touchEndTime muda quando vc der outro TouchEnd então é escalável (insano, descobri testando hooooly...)
        yield return new WaitUntil(() => tapCount >= MAX_TAP_COUNT || Time.time - touchEndTime >= tapDelayThreshold);
        switch (tapCount)
        {
            case 1:
                onSingleTap?.Invoke();
                break;
            case 2:
                onDoubleTap?.Invoke();
                break;
            case >= MAX_TAP_COUNT:
                onTripleTap?.Invoke();
                break;
        }

        tapCount = 0;
        handlingTaps = false;
    }

    #endregion

    #region swipe

    private void HandleSwipe(Vector2 startPosition, Vector2 endPosition)
    {
        Direction swipeDirection = CalculateSwipeDirection(startPosition, endPosition);
        onSwipe?.Invoke(swipeDirection);
    }

    private Direction CalculateSwipeDirection(Vector2 startPosition, Vector2 endPosition)
    {
        Vector2 resultVector = endPosition - startPosition;
        Direction swipeDirection;
        if (resultVector.y > resultVector.x)
        {
            swipeDirection = resultVector.y >= 0 ? Direction.UP : Direction.DOWN;
        }
        else
        {
            swipeDirection = resultVector.x >= 0 ? Direction.RIGHT : Direction.LEFT;
        }

        return swipeDirection;
    }

    #endregion

    #region tilt

    private void GetTilt()
    {
        onTilt?.Invoke(Input.acceleration);
    }

    #endregion
}