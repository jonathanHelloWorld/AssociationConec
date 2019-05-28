using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.Views.HUD
{
    [RequireComponent(typeof(Button))]
    public class ButtonHideOnClick : CanvasGroupView
    {
        protected Button _bt;

        protected override void OnAwake()
        {
            base.OnAwake();

            _bt = GetComponent<Button>();
            _bt.onClick.AddListener(OnClick);
        }

        protected virtual void OnClick()
        {
            _cv.DOFade(0, TransitionTime).Play();
            _cv.interactable = false;
            _cv.blocksRaycasts = false;
        }
    }
}