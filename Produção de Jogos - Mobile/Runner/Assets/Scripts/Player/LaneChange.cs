using Collectables;
using Managers;
using UnityEngine;

namespace Player
{
    public class LaneChange : MonoBehaviour
    {
        [SerializeField] private float laneDistance = 2.5f;
        private enum Lane : short
        {
            FarLeft = -2,
            Left = -1,
            Center = 0,
            Right = 1,
            FarRight = 2
        }

        private Lane currentLane = 0;
        public float targetLanePosition { get; private set; }

        private void Start()
        {
            targetLanePosition = transform.position.x;
            InputHandler.Instance.onLeftDirection += MoveLeft;
            InputHandler.Instance.onRightDirection += MoveRight;
        }

        private void OnDestroy()
        {
            if (InputHandler.Instance != null)
            {
                InputHandler.Instance.onLeftDirection -= MoveLeft;
                InputHandler.Instance.onRightDirection -= MoveRight;
            }
        }

        private void MoveLeft()
        {
            if (currentLane <= Lane.FarLeft) return;
            currentLane--;
            UpdateTargetPosition();
        }

        private void MoveRight()
        {
            if (currentLane >= Lane.FarRight) return;
            currentLane++;
            UpdateTargetPosition();
        }

        private void UpdateTargetPosition()
        {
            targetLanePosition = (short)currentLane * laneDistance;
        }
    }
}