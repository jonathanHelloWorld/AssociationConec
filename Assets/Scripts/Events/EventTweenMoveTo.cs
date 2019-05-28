using DG.Tweening;
using UnityEngine;

namespace InterativaSystem.Views.Events
{
    public class EventTweenMoveTo : ExecuteEvent
    {
        [Space(10f)]
        public Transform objectToTween;
        public Vector3 localPosition = Vector3.zero;
        public float duration = 5f;
        public Ease ease = Ease.InOutQuad;

        protected override void RunEvent()
        {
            TweenTo();
        }

        void TweenTo()
        {
            objectToTween.DOLocalMove(localPosition, 5f)
                            .SetEase(ease)
                            .OnStart(() => { EventStart(); })
                            .OnUpdate(() => { EventRepeat(); })
                            .OnComplete(() => { EventEnd(); })
                            .Play();
        }
    }
}
