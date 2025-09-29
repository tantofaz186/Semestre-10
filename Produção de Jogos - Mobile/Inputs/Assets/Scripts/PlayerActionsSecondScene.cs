using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerActionsSecondScene : PlayerActions
    {
        const int FULL_ROTATION = 360;

        #region unity functions

        protected override void Start()
        {
            base.Start();
            InputHandler.Instance.onSwipe += SwipeAction;
            InputHandler.Instance.onTilt += TiltAction;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            InputHandler.Instance.onSwipe -= SwipeAction;
            InputHandler.Instance.onTilt -= TiltAction;
        }

        #endregion

        #region actions

        private void SwipeAction(InputHandler.Direction dir)
        {
            if (isActing) return;
            StartCoroutine(Flip(dir));
        }

        private void TiltAction(Vector3 obj)
        {
            if (isActing) return;
            Move(obj);
        }

        #endregion

        #region flip

        public IEnumerator Flip(InputHandler.Direction dir)
        {
            isActing = true;
            Vector3 pivot = controller.bounds.min;
            Vector3 axis;
            switch (dir)
            {
                case InputHandler.Direction.UP:
                    axis = -transform.right;
                    break;
                case InputHandler.Direction.DOWN:
                    axis = transform.right;
                    break;
                case InputHandler.Direction.LEFT:
                    axis = transform.forward;
                    break;
                case InputHandler.Direction.RIGHT:
                    axis = -transform.forward;
                    break;
                default:
                    axis = Vector3.zero;
                    break;
            }

            float totalRotation = 0f;
            while (totalRotation < 360)
            {
                float frameRotation = Time.deltaTime * FULL_ROTATION;
                totalRotation += frameRotation;
                if (totalRotation > FULL_ROTATION)
                {
                    frameRotation = frameRotation - (totalRotation - FULL_ROTATION);
                    totalRotation = FULL_ROTATION;
                }

                controller.transform.RotateAround(pivot, axis, frameRotation);
                yield return null;
            }

            isActing = false;
        }

        #endregion

        #region move

        private void Move(Vector3 obj)
        {
            Vector3 movement = new Vector3(obj.x * speed, obj.z * GRAVITY_ACCELERATION, 0) * Time.deltaTime;
            controller.Move(movement);
        }

        #endregion
    }
}