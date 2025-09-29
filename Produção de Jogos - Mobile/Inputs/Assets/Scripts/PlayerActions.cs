using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class PlayerActions : MonoBehaviour
    {
        [SerializeField] protected float speed = 15f;
        [SerializeField] private Transform arms;
        [SerializeField] private Material blueMaterial;
        [SerializeField] private Material redMaterial;

        protected const float GRAVITY_ACCELERATION = 9.8f;
        protected CharacterController controller;
        private MeshRenderer mr;
        protected bool isActing = false;
        #region unity functions

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            mr = GetComponent<MeshRenderer>();
        }

        protected virtual void Start()
        {
            InputHandler.Instance.onSingleTap += SingleTapAction;
            InputHandler.Instance.onDoubleTap += DoubleTapAction;
            InputHandler.Instance.onTripleTap += TripleTapAction;
        }

        protected virtual void OnDisable()
        {
            InputHandler.Instance.onSingleTap -= SingleTapAction;
            InputHandler.Instance.onDoubleTap -= DoubleTapAction;
            InputHandler.Instance.onTripleTap -= TripleTapAction;
        }

        #endregion

        #region actions

        private void SingleTapAction()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex == 0 ? 1 : 0);
        }

        private void DoubleTapAction()
        {
            ChangeMaterial();
        }

        private void TripleTapAction()
        {
            if (isActing) return;
            StartCoroutine(Punch());
        }

        #endregion

        #region punch

        private IEnumerator Punch()
        {
            isActing = true;
            yield return new WaitUntil(MoveRightArmForward());
            yield return new WaitUntil(MoveRightArmBack());
            yield return new WaitUntil(MoveLeftArmForward());
            yield return new WaitUntil(MoveLeftArmBack());
            isActing = false;

        }

        private Func<bool> MoveRightArmForward()
        {
            return () =>
            {
                arms.GetChild(0).transform.position += Vector3.right * (5 * Time.deltaTime);
                return arms.GetChild(0).transform.localPosition.x >= 1;
            };
        }

        private Func<bool> MoveRightArmBack()
        {
            return () =>
            {
                arms.GetChild(0).transform.position -= Vector3.right * (5 * Time.deltaTime);
                return arms.GetChild(0).transform.localPosition.x <= 0;
            };
        }

        private Func<bool> MoveLeftArmForward()
        {
            return () =>
            {
                arms.GetChild(1).transform.position += Vector3.right * (5 * Time.deltaTime);
                return arms.GetChild(1).transform.localPosition.x >= 1;
            };
        }

        private Func<bool> MoveLeftArmBack()
        {
            return () =>
            {
                arms.GetChild(1).transform.position -= Vector3.right * (5 * Time.deltaTime);
                return arms.GetChild(1).transform.localPosition.x <= 0;
            };
        }

        #endregion

        #region change material

        private void ChangeMaterial()
        {
            mr.sharedMaterial = mr.sharedMaterial == redMaterial ? blueMaterial : redMaterial;
        }

        #endregion
    }
}