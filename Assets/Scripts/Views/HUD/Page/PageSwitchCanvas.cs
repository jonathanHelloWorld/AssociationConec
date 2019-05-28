using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Page
{
    public class PageSwitchCanvas : GenericView
    {
        public CanvasGroup[] Canvas;
        private int _actual;

        public float DelayTime;

        protected override void OnStart()
        {
            Show();
        }

        void Show()
        {
            float duration = 0;

            for (int i = 0, n = Canvas.Length -1; i < n; i++)
            {
                duration = Canvas[i].DOFade(1, 0.4f).SetDelay(duration + DelayTime).Play().Duration();
            }
            Canvas.Last().DOFade(1, 0.4f).SetDelay(duration + DelayTime).Play().OnComplete(Hide);
        }
        void Hide()
        {
            float duration = 0;

            for (int i = Canvas.Length - 1; i > 0; i--)
            {
                duration = Canvas[i].DOFade(0, 0.4f).SetDelay(duration + DelayTime).Play().Duration();
            }
            Canvas.First().DOFade(0, 0.4f).SetDelay(duration + DelayTime).Play().OnComplete(Show);
        }
    }
}