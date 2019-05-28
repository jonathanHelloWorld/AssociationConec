using DG.Tweening;

namespace InterativaSystem.Views.HUD
{
    public class CanvasChangeAuto : CanvasGroupView
    {
        public float Duration;
        public int Id, Count;
        
        private float _waitTime, _offTime;

        protected override void OnStart()
        {
            base.OnStart();
            _cv.alpha = 0;
            _cv.interactable = false;
            _cv.blocksRaycasts = false;

            _waitTime = (Duration + TransitionTime * 2);
            _offTime = 0;
            Invoke("Show", _waitTime * Id);
        }

        public override void Show()
        {
            _cv.DOFade(1, TransitionTime).SetDelay(_offTime).OnComplete(Hide);
            _offTime = _waitTime * Count;
        }
        public override void Hide()
        {
            _cv.DOFade(0, TransitionTime).SetDelay(Duration).OnComplete(Show);
        }
    }
}