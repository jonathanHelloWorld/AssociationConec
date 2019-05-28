using DG.Tweening;
using UnityEngine;

namespace InterativaSystem.Views.HUD.ActionCall
{
    public class CallCanvasOnAndTranslateById : CallCanvasOnById
    {
        public Transform Ini, End;
        public float DurationIn, DurationOut;
        protected override void In()
        {
            base.In();

            transform.DOPause();
            transform.DOMove(Ini.position, DurationIn).SetEase(Ease.InOutCubic).Play();
        }

        protected override void Out()
        {
            base.Out();

            transform.DOPause();
            transform.DOMove(End.position, DurationOut).SetEase(Ease.InOutCubic).Play();
        }
    }
}