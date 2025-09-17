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

        private void SwipeAction(InputHandler.Direction obj)
        {
            throw new NotImplementedException();
        }

        private void TiltAction(Vector2 obj)
        {
            throw new NotImplementedException();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            InputHandler.Instance.onSwipe -= SwipeAction;
            InputHandler.Instance.onTilt -= TiltAction;
        }
        }
    }
}