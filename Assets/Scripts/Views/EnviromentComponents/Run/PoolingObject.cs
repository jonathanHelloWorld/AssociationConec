using System.Threading;
using UnityEngine;

namespace InterativaSystem.Views.EnviromentComponents
{
    [RequireComponent(typeof(Collider))]
    public class PoolingObject : GenericView
    {
        protected Renderer[] _childs;
        protected Collider _collider;

        private bool _resetTimeout;

        public bool DoNothing;
        protected override void OnStart()
        {
            _childs = transform.GetComponentsInChildren<Renderer>();
            _collider = GetComponent<Collider>();
        }

        protected void Reset()
        {
            if (!_resetTimeout) return;

            _resetTimeout = false;
            SetVisibility(true);
        }

        public void OnTriggerEnter(Collider other)
        {
            SetVisibility(false);

            if (DoNothing)
            {
                new Thread(() =>
                {
                    Timeout();
                }).Start();
                return;
            }

            OnCollect(other.gameObject);
            PlaySound();

            new Thread(() =>
            {
                Timeout();
            }).Start();
        }

        void Timeout()
        {
            Thread.Sleep(650);

            _resetTimeout = true;
        }

        protected void SetVisibility(bool value)
        {
            if (_collider == null) _collider = GetComponent<Collider>();
            if (_childs == null) _childs = transform.GetComponentsInChildren<Renderer>();

            _collider.enabled = value;

            for (int i = 0, n = _childs.Length; i < n; i++)
            {
                _childs[i].enabled = value;
            }
        }

        protected virtual void PlaySound() { }

        protected virtual void OnCollect(GameObject other) { }
    }
}