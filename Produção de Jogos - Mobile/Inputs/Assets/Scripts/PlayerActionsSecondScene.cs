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

        private void DoAFlip(InputHandler.Direction dir)
        {
            switch (dir)
            {
                case InputHandler.Direction.UP:
                    FrontFlip();
                    break;
                case InputHandler.Direction.DOWN:
                    BackFlip();
                    break;
                case InputHandler.Direction.LEFT:
                    SideFlipLeft();
                    break;
                case InputHandler.Direction.RIGHT:
                    SideFlipRight();
                    break;
                default:
                    break;
            }
        }

        private void FrontFlip()
        {
            animator.SetTrigger(frontFlip);
        }   

        private void BackFlip()
        {
            animator.SetTrigger(backFlip);
        }

        private void SideFlipLeft()
        {
            animator.SetTrigger(leftFlip);
        }

        private void SideFlipRight()
        {
            animator.SetTrigger(rightFlip);
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