using UnityEngine;

namespace InterativaSystem.Views.HUD
{
    [RequireComponent(typeof(Renderer))]
    public class RendererView :GenericView
    {
        protected Renderer _renderer;

        public CanvasGroup ParentCanvas;

        protected override void OnAwake()
        {
            base.OnAwake();

            _renderer = GetComponent<Renderer>();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (_renderer == null || ParentCanvas == null) return;

            _renderer.enabled = ParentCanvas.interactable;
        }
    }
}