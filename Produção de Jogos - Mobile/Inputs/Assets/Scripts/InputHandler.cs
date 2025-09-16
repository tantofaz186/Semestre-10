using System;
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
    public event Action<Vector2> onTilt;

    [SerializeField]
    private float tapDelayThreshold = 0.4f;

    [SerializeField]
    private float swipeMinimalDistance = 0.25f;

    private Vector2 touchBeginPosition;
    private float touchEndTime;

    private void Update()
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
        }
    }

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
}