using System;
using InterativaSystem.Controllers.Run;
using UnityEngine;

namespace InterativaSystem.Views.ControllableCharacters
{
    public class RunSoundPitchBasedEngine : AudioSourceView
    {
        private RunController _runController;

        [Range(1,3)]
        public float MaxPitch = 2;
        [Range(0, 1)]
        public float MinPitch = 0.8f;

        protected override void OnStart()
        {
            base.OnStart();
            _runController = _controller as RunController;

            StartEngine();
            _runController.OnGameEnd += StopEngine;
        }

        protected override void OnDestroied()
        {
            base.OnDestroied();
            _runController.OnGameEnd -= StopEngine;
        }



        private void StopEngine()
        {
            StopSound();
        }

        private void StartEngine()
        {
            PlaySound("Engine");
            _audioSource.pitch = MinPitch;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            var pitch = (_runController.Speed / _runController.FixedSpeed) * (MaxPitch - MinPitch) + MinPitch;

            _audioSource.pitch = pitch;
        }
    }
}