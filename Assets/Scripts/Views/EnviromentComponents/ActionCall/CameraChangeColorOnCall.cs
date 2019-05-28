using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace InterativaSystem.Views.EnviromentComponents.ActionCall
{
    [RequireComponent(typeof(Camera))]
    public class CameraChangeColorOnCall : GenericView
    {
        public int Id;

        public Color On = Color.black, Off = Color.white;

        public float Delay, Duration;

        public bool EnableOnLastActionEnded;
        public bool OnlyReturnOnActionEnded;
        public bool DisableOnActionStart;

        private Camera _camera;

        protected override void OnStart()
        {
            base.OnStart();

            _camera = GetComponent<Camera>();

            _controller.CallGenericAction += CheckShow;
            _controller.GenericActionEnded += Disable;

            if (DisableOnActionStart)
                _controller.CallGenericAction += Disable;

            if (EnableOnLastActionEnded)
                _controller.GenericActionEnded += ShowBefore;
        }
        private void ShowBefore(int value)
        {
            if (Id - 1 != value) return;

            Enable(value);
        }

        private void CheckShow(int value)
        {
            //if (Ids.Count > 0 && Ids.Contains(value) && Id != value) return;

            if (Id == value)
                Enable(value);
            else if (!OnlyReturnOnActionEnded)
                Disable(value);
        }

        void Enable(int value)
        {
            _camera.DOColor(On, Duration).SetDelay(Delay).Play();
        }

        public void Disable(int value)
        {
            _camera.DOColor(Off, Duration).SetDelay(Delay).Play();
        }
    }
}