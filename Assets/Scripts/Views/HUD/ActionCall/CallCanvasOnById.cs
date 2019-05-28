using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace InterativaSystem.Views.HUD.ActionCall
{
    public class CallCanvasOnById : CanvasGroupView
    {
        public float Delay, DelayOff;

        public bool NeverHide;

        [Space]
        public int Id;
        public List<int> Ids;
        public bool EnableOnLastActionEnded;
        public bool OnlyReturnOnActionEnded;
        public bool DisableOnActionStart;

        protected override void OnStart()
        {
            base.OnStart();

            _controller.CallGenericAction += CheckShow;
            _controller.GenericActionEnded += Hide;

            if (DisableOnActionStart)
                _controller.CallGenericAction += Hide;

            if (EnableOnLastActionEnded)
                _controller.GenericActionEnded += ShowBefore;

            if (Ids != null && Ids.Count > 0)
            {
                _cv.DOFade(Ids.Contains(0) ? 1 : 0, TransitionTime).Play();
                _cv.interactable = Ids.Contains(0);
                _cv.blocksRaycasts = Ids.Contains(0);
            }
            else
            {
                _cv.DOFade(Id == 0 ? 1 : 0, TransitionTime).Play();
                _cv.interactable = Id == 0;
                _cv.blocksRaycasts = Id == 0;
            }
        }

        protected virtual void In() { }
        protected virtual void Out() { }

        private void ShowBefore(int value)
        {
            if (Ids != null && Ids.Count > 0)
            {
                if(Ids.Exists(x=>x-1 != value)) return;
            }
            else
            {
                if (Id - 1 != value) return;
            }

            _cv.DOFade(1, TransitionTime).SetDelay(Delay).Play();
            _cv.interactable = true;
            _cv.blocksRaycasts = true;

            In();
        }

        private void CheckShow(int value)
        {
            var result = Id == value;
            if (Ids != null && Ids.Count > 0)
            {
                if (Ids.Exists(x => NeverHide && x != value)) return;
                result = Ids.Contains(value);
            }
            else
            {
                if (NeverHide && Id != value) return;
                result = Id == value;
            }

            if (!result && OnlyReturnOnActionEnded) return;

            _cv.DOFade(result ? 1 : 0, TransitionTime).SetDelay(result ? Delay : DelayOff).Play();
            _cv.interactable = result;
            _cv.blocksRaycasts = result;

            if(result)
                In();
            else
                Out();
        }

        private void Hide(int value)
        {
            if (Ids != null && Ids.Count > 0)
            {
                if (Ids.Exists(x => NeverHide || x != value)) return;
            }
            else
            {
                if (Id != value || NeverHide) return;
            }

            _cv.DOFade(0, TransitionTime).SetDelay(DelayOff).Play();
            _cv.interactable = false;
            _cv.blocksRaycasts = false;

            Out();
        }
    }
}