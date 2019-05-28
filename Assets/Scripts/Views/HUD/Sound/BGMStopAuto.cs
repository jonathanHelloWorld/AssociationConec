using System.Collections;
using InterativaSystem;
using InterativaSystem.Controllers;
using InterativaSystem.Views;
using InterativaSystem.Views.HUD;
using InterativaSystem.Views.HUD.Page;
using UnityEngine;
using InterativaSystem.Controllers.Sound;
using InterativaSystem.Models;

namespace Assets.Scripts.Views.HUD.Sound
{
    [RequireComponent(typeof(Page))]
    public class BGMStopAuto : DoOnPageAuto
    {
        private BGMController _bgmController;

        protected override void OnStart()
        {
            base.OnStart();

            _bgmController = _bootstrap.GetController(ControllerTypes.SoundBGM) as BGMController;
        }

        protected override void DoSomething()
        {
            _bgmController.StopSound();
        }
    }
}
