using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class PlayerActions : MonoBehaviour
    {
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
            throw new NotImplementedException();
        }
        private void TripleTapAction()
        {
            throw new NotImplementedException();
        }

        protected virtual void OnDisable()
        {
            InputHandler.Instance.onSingleTap -= SingleTapAction;
            InputHandler.Instance.onDoubleTap -= DoubleTapAction;
            InputHandler.Instance.onTripleTap -= TripleTapAction;
        }


    }
}