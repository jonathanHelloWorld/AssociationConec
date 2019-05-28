using System.Security.Policy;
using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Sound;
using InterativaSystem.Views.HUD;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Game
{
    public class GameCounter : ImageView
    {
        public Sprite[] Counter;

        private SFXController _sfx;

        private float _timeflow;

        protected override void OnStart()
        {
            _controller.OnGameStart += StartCount;

            _timeflow = UnityEngine.Time.realtimeSinceStartup;

            _sfx = _bootstrap.GetController(ControllerTypes.SoundSFX) as SFXController;

            _image.color = new Color(0, 0, 0, 0);
        }

        private bool hasCounted;
        private void StartCount()
        {
            hasCounted = false;
            _timeflow = UnityEngine.Time.realtimeSinceStartup;
        }

        protected override void OnUpdate()
        {
            if (hasCounted || !_controller.IsGameStarted || !_controller.IsGameRunning || !_bootstrap.IsAppRunning) return;

            var time = UnityEngine.Time.realtimeSinceStartup - _timeflow;

            for (int i = 1, n = Counter.Length; i <= n; i++)
            {
                if (time <= i && time > i - 1)
                {
                    _image.color = Color.white;
                    _image.sprite = Counter[i - 1];

                    break;
                }
                _image.color = new Color(0, 0, 0, 0);
            }

            if (time >= Counter.Length)
            {
                _sfx.PlaySound("CounterEnd");
                hasCounted = true;
            }
        }
    }
}