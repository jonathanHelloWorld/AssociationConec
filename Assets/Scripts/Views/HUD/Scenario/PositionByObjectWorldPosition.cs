using InterativaSystem.Models;
using UnityEngine;

namespace InterativaSystem.Views.HUD
{
    public class PositionByObjectWorldPosition : GenericView
    {
        private ScreenInfo _screenInfo;
        public Transform Target;
        private RectTransform _rect;

        protected override void OnStart()
        {
            base.OnStart();

            _screenInfo = _bootstrap.GetModel(ModelTypes.Screen) as ScreenInfo;

            _rect = gameObject.GetComponent<RectTransform>();

            _rect.anchorMax = new Vector2(0.5f, 0.5f);
            _rect.anchorMin = new Vector2(0.5f, 0.5f);

            var pos = _screenInfo.GetWordPosToScreen(Target);
            _rect.localPosition = pos;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            var pos = _screenInfo.GetWordPosToScreen(Target);
            _rect.localPosition = pos;
        }
    }
}