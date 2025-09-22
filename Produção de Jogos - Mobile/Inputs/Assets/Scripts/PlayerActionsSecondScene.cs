using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerActionsSecondScene : PlayerActions
    {
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
            throw new NotImplementedException();
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