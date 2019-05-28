using DG.Tweening;
using UnityEngine;

namespace InterativaSystem.Views.EnviromentComponents
{
    public class CameraBumpOnCollision : GenericView
    {
        [Header("Settings")]
        public float BumpDuration = 0.4f;
        public Vector3 BumpForce;
        private Vector3 _iniRot;

        protected override void OnStart()
        {
            _controller.ObstacleCollision += OnCollided;

            _iniRot = transform.localEulerAngles;
        }

        void OnCollided()
        {
            BumpCamera();
        }

        void BumpCamera()
        {
            var duration = transform.DOShakeRotation(BumpDuration, BumpForce).Play().Duration();
            transform.DOLocalRotate(_iniRot, 0.4f).SetDelay(duration).Play();
        }
    }
}