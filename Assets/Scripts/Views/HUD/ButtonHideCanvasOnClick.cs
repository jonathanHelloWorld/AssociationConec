using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.Views.HUD
{
    public class ButtonHideCanvasOnClick : ButtonView
    {
        public CanvasGroup Canvas;

        [Header("Transition")]
        public float TransitionTime = 0.4f;

        protected override void OnClick()
        {
            Canvas.DOFade(0, TransitionTime).Play();
            Canvas.interactable = false;
            Canvas.blocksRaycasts = false;
        }
    }
}