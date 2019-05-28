using System;
using DG.Tweening;
using UnityEngine;

namespace InterativaSystem.Views.HUD
{
    public class PanelChangeBackground : ImageView
    {
        public Sprite[] Backgrounds;
        [HideInInspector]
        public int _actualBg;

        public bool HideOnStart;
        private CanvasGroup _cv;
        public float TransitionTime = 0.4f;

        protected override void OnAwake()
        {
            base.OnAwake();

            _cv = GetComponent<CanvasGroup>();
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (HideOnStart && _cv != null)
            {
                _cv.DOFade(0, TransitionTime).Play();
                _cv.interactable = false;
                _cv.blocksRaycasts = false;
            }
        }

        public void NextBg(bool next)
        {
            if (next)
            {
                _actualBg += _actualBg != Backgrounds.Length - 1 ? 1 : 0;
            }
            else
            {
                _actualBg -= _actualBg != 0 - 1 ? 1 : 0;
            }

            _image.sprite = Backgrounds[_actualBg];
        }

        protected override void ResetView()
        {
            base.ResetView();

            if (HideOnStart && _cv != null)
            {
                _cv.DOFade(0, TransitionTime).Play();
                _cv.interactable = false;
                _cv.blocksRaycasts = false;
            }

            _actualBg = 0;
            _image.sprite = Backgrounds[_actualBg];
        }
    }
}