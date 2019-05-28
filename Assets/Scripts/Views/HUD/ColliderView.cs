using UnityEngine;

namespace InterativaSystem.Views.HUD
{
    [RequireComponent(typeof(Collider))]
    public class ColliderView : GenericView
    {
        protected Collider _collider;

        public CanvasGroup ParentCanvas;

        protected override void OnAwake()
        {
            base.OnAwake();

            _collider = GetComponent<Collider>();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (ParentCanvas == null) return;

            _collider.enabled = ParentCanvas.interactable;
        }
    }
}