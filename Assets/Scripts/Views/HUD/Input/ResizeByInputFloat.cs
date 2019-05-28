using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Input
{
    public class ResizeByInputFloat : ImageView
    {
        protected InputController _inputController;
        public int FloatId;

        public bool Height, Weight;

        public float maxSize, minSize;

        private RectTransform rectTransform;

        protected override void OnStart()
        {
            base.OnStart();

            _inputController = _controller as InputController;
            rectTransform = GetComponent<RectTransform>();
        }

        protected override void OnLateUpdate()
        {
            base.OnUpdate();

            if (_inputController.FloatControllers.Count < FloatId) return;

            rectTransform.sizeDelta = new Vector2(
                Weight ? Mathf.Lerp(minSize, maxSize, _inputController.FloatControllers[FloatId]) : rectTransform.sizeDelta.x, 
                Height ? Mathf.Lerp(minSize, maxSize, _inputController.FloatControllers[FloatId]) : rectTransform.sizeDelta.y
                );
        }
    }
}