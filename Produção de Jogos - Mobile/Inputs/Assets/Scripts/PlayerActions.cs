using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class PlayerActions : MonoBehaviour
    {
        [SerializeField] protected float speed = 15f;

        protected const float GRAVITY_ACCELERATION = 9.8f;
        protected CharacterController controller;

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

        private void Kick()
        {
            throw new NotImplementedException();
        }

        private void Punch()
        {
            throw new NotImplementedException();
        }
    }
}