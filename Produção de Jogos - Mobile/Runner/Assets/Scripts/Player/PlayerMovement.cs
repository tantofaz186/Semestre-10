using Managers;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Collider), typeof(LaneChange))]
    public class PlayerMovement : MonoBehaviour
    {
                
        [SerializeField] private float startingForwardSpeed = 20f;
        [SerializeField] private float startingLaneChangeSpeed = 20f;

        private float forwardSpeed => startingForwardSpeed * (1 + difficultyMultiplier);
        private float laneChangeSpeed => startingLaneChangeSpeed * (1 + difficultyMultiplier);
        private float difficultyMultiplier => transform.position.z * 0.002f;


        private LaneChange laneChange;

        #region unity functions

        private void Awake()
        {
            laneChange = GetComponent<LaneChange>();
        }

        private void Start()
        {
            GameManager.Instance.player = this;
        }

        private void Update()
        {
            Move();
        }

        #endregion

        public void Move()
        {
            MoveForward();
            MoveLanes();
        }

        private void MoveLanes()
        {
            Vector3 position = transform.position;
            Vector3 targetPosition = new Vector3(laneChange.targetLanePosition, position.y, position.z);
            transform.position = Vector3.MoveTowards(position, targetPosition, laneChangeSpeed * Time.deltaTime);
        }

        private void MoveForward()
        {
            transform.Translate(Vector3.forward * (forwardSpeed * Time.deltaTime));
        }

        public void ApplySlow()
        {
            startingForwardSpeed /= 2;
            startingLaneChangeSpeed /= 2;
            Invoke(nameof(RemoveSlow), 1.5f);
        }

        private void RemoveSlow()
        {
            startingForwardSpeed *= 2;
            startingLaneChangeSpeed *= 2;
        }
    }
}