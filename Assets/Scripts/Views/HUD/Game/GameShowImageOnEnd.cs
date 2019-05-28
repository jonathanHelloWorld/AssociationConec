using DG.Tweening;
using InterativaSystem.Views.HUD;
using UnityEngine;

namespace Assets.Scripts.Views.HUD.Game
{
    public class GameShowImageOnEnd : ImageView
    {
        protected override void OnStart()
        {
            _controller.OnGameEnd += Show;
            _controller.OnGamePrepare += Hide;

            Hide();
        }

        private void Hide()
        {
            _image.enabled = false;
            _image.DOFade(0, 0.1f);
        }

        private void Show()
        {
            _image.enabled = true;
            _image.DOFade(1, 0.4f);
        }
    }
}