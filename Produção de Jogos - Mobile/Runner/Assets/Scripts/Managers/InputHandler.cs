using System;
using UnityEngine;

namespace Managers
{
    public class InputHandler : MonoBehaviour
    {
        public event Action onLeftDirection;
        public event Action onRightDirection;

        [SerializeField] private float swipeMinimalDistance = 75f;

        private Vector2 touchBeginPosition;

        public static InputHandler Instance { get; private set; }

        #region unity functions

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
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
            HandleKeyboard();
        }

        #endregion

        #region touch

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

        private void HandleTouchBegin(Touch touch)
        {
            touchBeginPosition = touch.position;
        }

        private void HandleTouchEnd(Touch touch)
        {
            if (Vector3.Distance(touchBeginPosition, touch.position) > swipeMinimalDistance) HandleSwipe(touchBeginPosition, touch.position);
        }

        #endregion

        #region swipe

        private void HandleSwipe(Vector2 startPosition, Vector2 endPosition)
        {
            Vector2 resultVector = endPosition - startPosition;
            if (resultVector.x > 0)
                PressedRight();
            else
                PressedLeft();
        }

        #endregion

        #region keyboard

        private void HandleKeyboard()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                PressedLeft();
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) PressedRight();
        }

        #endregion

        #region eventInvokers

        public void PressedLeft()
        {
            onLeftDirection?.Invoke();
        }

        public void PressedRight()
        {
            onRightDirection?.Invoke();
        }

        #endregion
    }
}