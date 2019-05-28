using InterativaSystem;
using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Sound;
using InterativaSystem.Views;
using UnityEngine;

namespace InterativaSystem.Views.Events.Functions
{
    public class FuncPlayBGM : GenericEvent
    {
        [Space(10f)]
        public string audioName = "BGM";

        BGMController _bgmController;

        protected override void OnStart()
        {
            base.OnStart();

            _bgmController = _controller as BGMController;
        }

        protected override void RunEvent()
        {
            _bgmController.PlaySound(audioName);
        }
    }
}