using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.Views.HUD
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupView : GenericView
    {
        protected CanvasGroup _cv;

        [Header("Transition")] 
        public float TransitionTime = 0.4f;

        protected override void OnAwake()
        {
            base.OnAwake();

            _cv = GetComponent<CanvasGroup>();
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (_cv == null)
            _cv = GetComponent<CanvasGroup>();
        }

        public virtual void Show()
        {
            _cv.DOFade(1, TransitionTime);
            _cv.blocksRaycasts = true;
            _cv.interactable = true;
        }
        public virtual void Hide()
        {
            _cv.DOFade(0, TransitionTime);
            _cv.blocksRaycasts = false;
            _cv.interactable = false;
        }
    }
}