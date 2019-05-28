using System;
using DG.Tweening;
using InterativaSystem.Views.HUD;
using UnityEngine;

namespace Assets.Scripts.Views.HUD.Effects
{
    public class EffectRotateOverTime : ImageView
    {
        public float Duration;
        public Vector3 Amount;
        public Ease Ease;
        public bool Relative;

        protected override void OnStart()
        {
            _image.rectTransform.DORotate(Amount, Duration)
                .SetEase(Ease)
                .SetUpdate(UpdateType.Fixed)
                .OnComplete(RotateUpdate)
                .SetRelative(Relative)
                .Play();
        }
        void RotateUpdate()
        {
            var delay = _image.rectTransform.DORotate(Amount * -2, Duration*2)
                .SetEase(Ease)
                .SetUpdate(UpdateType.Fixed)
                .SetRelative(Relative)
                .Play()
                .Duration();

            _image.rectTransform.DORotate(Amount * 2, Duration * 2)
                .SetEase(Ease)
                .SetUpdate(UpdateType.Fixed)
                .SetRelative(Relative)
                .OnComplete(RotateUpdate)
                .SetDelay(delay)
                .Play();
        }
    }
}