﻿using System.Collections;
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
    public class BGMPlayAuto : DoOnPageAuto
    {
        private BGMController _bgmController;

        public int audioID = 0;
        public bool repeatable = false;
        bool isPlaying = false;

        protected override void OnStart()
        {
            base.OnStart();

            _bgmController = _bootstrap.GetController(ControllerTypes.SoundBGM) as BGMController;
        }

        protected override void DoSomething()
        {
            if (isPlaying && !repeatable) return;

            isPlaying = true;
            _bgmController.PlaySound(audioID);
        }
    }
}
