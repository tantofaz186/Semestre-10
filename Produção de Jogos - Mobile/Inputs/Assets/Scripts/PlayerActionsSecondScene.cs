using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerActionsSecondScene : PlayerActions
    {
        private static readonly int frontFlip = Animator.StringToHash("frontFlip");
        private static readonly int backFlip = Animator.StringToHash("backFlip");
        private static readonly int leftFlip = Animator.StringToHash("leftFlip");
        private static readonly int rightFlip = Animator.StringToHash("rightFlip");

        protected override void Start()
        {
            base.Start();
            InputHandler.Instance.onSwipe += SwipeAction;
            InputHandler.Instance.onTilt += TiltAction;
        }

        private void SwipeAction(InputHandler.Direction dir)
        {
            DoAFlip(dir);
        }
        bool isFlipping = false;
        private void DoAFlip(InputHandler.Direction dir)
        {
            if (isFlipping) return;
            StartCoroutine(Flip(dir));

        }

        public IEnumerator Flip(InputHandler.Direction dir)
        {
            isFlipping = true;
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
            Debug.Log(dir);
            yield return new WaitForSeconds(0.2f);
            controller.transform.RotateAround(pivot, axis, 90f);
            yield return new WaitForSeconds(0.2f);
            controller.transform.RotateAround(pivot, axis, 90f);
            yield return new WaitForSeconds(0.2f);
            controller.transform.RotateAround(pivot, axis, 90f);
            yield return new WaitForSeconds(0.2f);
            controller.transform.RotateAround(pivot, axis, 90f);
            isFlipping = false;
        }
        private void TiltAction(Vector3 obj)
        {
            Vector3 movement = new Vector3(obj.x * speed, obj.z * GRAVITY_ACCELERATION, 0) * Time.deltaTime;
            controller.Move(movement);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            InputHandler.Instance.onSwipe -= SwipeAction;
            InputHandler.Instance.onTilt -= TiltAction;
        }
    }
}