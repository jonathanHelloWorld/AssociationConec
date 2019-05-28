using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace InterativaSystem.Views.EnviromentComponents.ActionCall
{
    public class TweenOnActionCall : GenericView
    {
        public Transform Target;
        public Transform[] Targets;
        public float TransitionTime = 1;
        public Ease Ease;

        public bool Rotate = true;
        public bool Translate = true;

        [Space]
        public float Delay;

        [Space]
        public int Id;
        public List<int> Ids;
        public bool EnableOnLastActionEnded;
        public bool OnlyReturnOnActionEnded;
        public bool DisableOnActionStart;
        public bool LoopBack;

        private Vector3 _iniPos, _iniRot;

        protected override void OnAwake()
        {
            base.OnAwake();
            _iniPos = transform.position;
            _iniRot = transform.eulerAngles;
        }

        protected override void OnStart()
        {
            base.OnStart();

            _controller.CallGenericAction += CheckShow;
            _controller.GenericActionEnded += GoBack;

            if (DisableOnActionStart)
                _controller.CallGenericAction += GoBack;

            if (EnableOnLastActionEnded)
                _controller.GenericActionEnded += ShowBefore;
        }

        private void ShowBefore(int value)
        {
            if (Id - 1 != value) return;

            Go(value);
        }

        private void CheckShow(int value)
        {
            //if (Ids.Count > 0 && Ids.Contains(value) && Id != value) return;

            if (Ids.Count > 0 && Ids.Contains(value) || Id == value)
                Go(value);
            else if (!OnlyReturnOnActionEnded)
                GoBack(value);
        }

        public void Go(int value)
        {
            //DOTween.PauseAll();
            if (Ids.Count > 0)
            {
                var id = Ids.FindIndex(x => x == value);
                
                if(Rotate)
                    transform.DORotate(Targets[id].eulerAngles, TransitionTime).SetDelay(Delay).SetEase(Ease).Play().OnComplete(Completed);
                if (Translate)
                    transform.DOMove(Targets[id].position, TransitionTime).SetDelay(Delay).SetEase(Ease).Play().OnComplete(Completed);
            }
            else
            {
                if (Rotate)
                    transform.DORotate(Target.eulerAngles, TransitionTime).SetDelay(Delay).SetEase(Ease).Play().OnComplete(Completed);
                if (Translate)
                    transform.DOMove(Target.position, TransitionTime).SetDelay(Delay).SetEase(Ease).Play().OnComplete(Completed);
            }
        }

        private void Completed()
        {
            if (LoopBack)
                GoBack(-1);
        }

        public void GoBack(int value)
        {
            //if (Id != value) return;
            if (OnlyReturnOnActionEnded && !Ids.Contains(value)) return;

            //Debug.Log(value);

            //DOTween.PauseAll();
            if (Rotate)
                transform.DORotate(_iniRot, TransitionTime).SetEase(Ease).Play();
            if (Translate)
                transform.DOMove(_iniPos, TransitionTime).SetEase(Ease).Play();
        }
    }
}