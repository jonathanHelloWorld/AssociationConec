using UnityEngine;

namespace InterativaSystem.Views.Events
{
    public class FuncToggleColliders : GenericEvent
    {
        [Space(10f)]
        public Collider[] colliders;
        public bool toggle;

        protected override void OnStart()
        {
            base.OnStart();

            _bootstrap.Reset += ResetView;
        }

        protected override void RunEvent()
        {
            ToggleColliders();
        }

        void ToggleColliders()
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = toggle;
            }
        }

        protected override void ResetView()
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = !toggle;
            }
        }
    }
}
