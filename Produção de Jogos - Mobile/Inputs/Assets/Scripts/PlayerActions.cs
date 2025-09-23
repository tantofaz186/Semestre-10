using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class PlayerActions : MonoBehaviour
    {
        [SerializeField] protected float speed = 15f;

        protected const float GRAVITY_ACCELERATION = 9.8f;
        protected CharacterController controller;

        [SerializeField] protected Animator animator;
        private static readonly int punch = Animator.StringToHash("punch");
        private static readonly int kick = Animator.StringToHash("kick");
        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        protected virtual void Start()
        {
            InputHandler.Instance.onSingleTap += SingleTapAction;
            InputHandler.Instance.onDoubleTap += DoubleTapAction;
            InputHandler.Instance.onTripleTap += TripleTapAction;
        }

        private void SingleTapAction()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex == 0 ? 1 : 0);
        }

        private void DoubleTapAction()
        {
            Kick();
        }

        private void TripleTapAction()
        {
            Punch();
        }

        protected virtual void OnDisable()
        {
            InputHandler.Instance.onSingleTap -= SingleTapAction;
            InputHandler.Instance.onDoubleTap -= DoubleTapAction;
            InputHandler.Instance.onTripleTap -= TripleTapAction;
        }

        private void Punch()
        {
            animator.SetTrigger(punch);
        }

        private void Kick()
        {
            animator.SetTrigger(kick);
        }
    }
}