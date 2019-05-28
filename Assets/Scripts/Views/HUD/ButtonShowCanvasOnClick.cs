using DG.Tweening;
using UnityEngine;

namespace InterativaSystem.Views.HUD
{
    public class ButtonShowCanvasOnClick : ButtonView
    {
        public CanvasGroup Canvas;
        public bool Show = true;

        [Header("Transition")]
        public float TransitionTime = 0.4f;

        protected override void OnClick()
        {
            if (Show)
            {
                Canvas.DOFade(1, TransitionTime).Play();
                Canvas.interactable = true;
                Canvas.blocksRaycasts = true;
            }
            else
            {
                Canvas.DOFade(0, TransitionTime).Play();
                Canvas.interactable = false;
                Canvas.blocksRaycasts = false;
            }
        }
    }
}